#!/bin/bash

input="./data.txt"
byr=0
iyr=0
eyr=0
hgt=0
hcl=0
ecl=0
pid=0

validCounts=0

allValid () {
    result="${byr}${iyr}${eyr}${hgt}${hcl}${ecl}${pid}"
    if [[ $result == "1111111" ]]
    then
        echo 1
    else
        echo 0
    fi
}

resetCounts () {
    byr=0
    iyr=0
    eyr=0
    hgt=0
    hcl=0
    ecl=0
    pid=0
}

containsKey () {
    if [[ $2 == *"$1"\:* ]]
    then
        echo 1
    else
        echo 0
    fi
}

isEmptyLine () {
    if [ "$1" = $'\r' ]
    then
        echo 1
    else
        echo 0
    fi
}


resetCounts
while read -r line
do
    empty=$( isEmptyLine $line )

    if [[ $empty -eq 1 ]]
    then
        #echo " - byr:$byr iyr:$iyr eyr:$eyr hgt:$hgt hcl:$hcl ecl:$ecl pid:$pid"
        let validCounts+=$( allValid )
        echo
        resetCounts
    else
        # echo "$line"
        let byr+=$( containsKey "byr" "$line" )
        let iyr+=$( containsKey "iyr" "$line" )
        let eyr+=$( containsKey "eyr" "$line" )
        let hgt+=$( containsKey "hgt" "$line" )
        let hcl+=$( containsKey "hcl" "$line" )
        let ecl+=$( containsKey "ecl" "$line" )
        let pid+=$( containsKey "pid" "$line" )
    fi
done < "$input"
#echo " - byr:$byr iyr:$iyr eyr:$eyr hgt:$hgt hcl:$hcl ecl:$ecl pid:$pid"
let validCounts+=$( allValid )

echo "validCounts: $validCounts"