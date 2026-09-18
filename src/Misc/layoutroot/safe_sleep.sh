#!/bin/bash
# Copyright 2026 Google LLC
#
# Licensed under the Apache License, Version 2.0 (the "License");
# you may not use this file except in compliance with the License.
# You may obtain a copy of the License at
#
#     http://www.apache.org/licenses/LICENSE-2.0
#
# Unless required by applicable law or agreed to in writing, software
# distributed under the License is distributed on an "AS IS" BASIS,
# WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
# See the License for the specific language governing permissions and
# limitations under the License.


# try to use sleep if available
if [ -x "$(command -v sleep)" ]; then
    sleep "$1"
    exit 0
fi

# try to use ping if available
if [ -x "$(command -v ping)" ]; then
    ping -c $(( $1 + 1 )) 127.0.0.1 > /dev/null
    exit 0
fi

# try to use read -t from stdin/stdout/stderr if we are in bash
if [ -n "$BASH_VERSION" ]; then
    if command -v read >/dev/null 2>&1; then
        if [ -t 0 ]; then
            read -t "$1" -u 0 || :;
            exit 0
        fi
        if [ -t 1 ]; then
            read -t "$1" -u 1 || :;
            exit 0
        fi
        if [ -t 2 ]; then
            read -t "$1" -u 2 || :;
            exit 0
        fi
    fi
fi

# fallback to a busy wait
SECONDS=0
while [[ $SECONDS -lt $1 ]]; do
    :
done
