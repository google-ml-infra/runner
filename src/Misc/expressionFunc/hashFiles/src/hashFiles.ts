/**
 * Copyright 2026 Google LLC
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

import * as crypto from 'crypto'
import * as fs from 'fs'
import * as glob from '@actions/glob'
import * as path from 'path'
import * as stream from 'stream'
import * as util from 'util'

async function run(): Promise<void> {
  // arg0 -> node
  // arg1 -> hashFiles.js
  // env[followSymbolicLinks] = true/null
  // env[patterns] -> glob patterns
  let followSymbolicLinks = false
  const matchPatterns = process.env.patterns || ''
  if (process.env.followSymbolicLinks === 'true') {
    console.log('Follow symbolic links')
    followSymbolicLinks = true
  }

  console.log(`Match Pattern: ${matchPatterns}`)
  let hasMatch = false
  const githubWorkspace = process.cwd()
  const result = crypto.createHash('sha256')
  let count = 0
  const globber = await glob.create(matchPatterns, {followSymbolicLinks})
  for await (const file of globber.globGenerator()) {
    console.log(file)
    if (!file.startsWith(`${githubWorkspace}${path.sep}`)) {
      console.log(`Ignore '${file}' since it is not under GITHUB_WORKSPACE.`)
      continue
    }
    if (fs.statSync(file).isDirectory()) {
      console.log(`Skip directory '${file}'.`)
      continue
    }
    const hash = crypto.createHash('sha256')
    const pipeline = util.promisify(stream.pipeline)
    await pipeline(fs.createReadStream(file), hash)
    result.write(hash.digest())
    count++
    if (!hasMatch) {
      hasMatch = true
    }
  }
  result.end()

  if (hasMatch) {
    console.log(`Found ${count} files to hash.`)
    console.error(`__OUTPUT__${result.digest('hex')}__OUTPUT__`)
  } else {
    console.error(`__OUTPUT____OUTPUT__`)
  }
}

;(async () => {
  try {
    const out = await run()
    console.log(out)
    process.exit(0)
  } catch (err) {
    console.error(err)
    process.exit(1)
  }
})()
