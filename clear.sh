#!/bin/bash
CURRENT_DIR=$(cd $(dirname $0); pwd)

find ${CURRENT_DIR} -name 'bin' -type d  | xargs rm -rf
find ${CURRENT_DIR} -name 'obj' -type d  | xargs rm -rf