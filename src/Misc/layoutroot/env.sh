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


varCheckList=(
    'LANG' 
    'JAVA_HOME' 
    'ANT_HOME' 
    'M2_HOME' 
    'ANDROID_HOME' 
    'ANDROID_SDK_ROOT'
    'GRADLE_HOME' 
    'NVM_BIN' 
    'NVM_PATH'
    'LD_LIBRARY_PATH'
    'PERL5LIB'
    )

envContents=""

if [ -f ".env" ]; then
    envContents=`cat .env`
else
    touch .env
fi

function writeVar()
{
    checkVar="$1"
    checkDelim="${1}="
    if test "${envContents#*$checkDelim}" = "$envContents"
    then
        if [ ! -z "${!checkVar}" ]; then
            echo "${checkVar}=${!checkVar}">>.env
        fi
    fi 
}

echo $PATH>.path

for var_name in ${varCheckList[@]}
do
    writeVar "${var_name}"
done
