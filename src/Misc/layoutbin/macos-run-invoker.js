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

const { spawn } = require('child_process');
// argv[0] = node
// argv[1] = macos-run-invoker.js
var shell = process.argv[2];
var args = process.argv.slice(3);
console.log(`::debug::macos-run-invoker: ${shell}`);
console.log(`::debug::macos-run-invoker: ${JSON.stringify(args)}`);
var launch = spawn(shell, args, { stdio: 'inherit' });
launch.on('exit', function (code) {
    if (code !== 0) {
        process.exit(code);
    }
});
