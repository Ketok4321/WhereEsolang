PROJECT_PATH="../src/WhereEsolang.Cli"
BUILD_PATH="$PROJECT_PATH/bin/Debug/net6.0/WhereEsolang.Cli"

dotnet build $PROJECT_PATH
if [[ $? != 0 ]]; then
    echo -e "\e[91mBuild failed"
    exit 1
fi

test() {
    res=$($BUILD_PATH $1)
    if [[ $res = $2 ]]
    then
        echo -e "\e[92m'$1' passed"
    else
        echo -e "\e[91m'$1' failed (expected $2, got $res)"
    fi
}

test "./output.wh" "1"
test "./comments.wh" "1"
test "./while.wh" "10"
test "./nestedwhile.wh" "10"
