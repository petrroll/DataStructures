#!/bin/bash

output="graph.svg"
title="Average path length on Find in SplayTree"
logScale=""
path=$(pwd)
scale=1
barsEvery=20
while [ "$1" != "" ]; do
	case "$1" in
	-logFileNames)
        shift
	    logFileNames=$1
	    ;;
	-legendNames)
	    shift
	    legendNames=$1
	    ;;
	-output)
	    shift
	    output=$1
	    ;;
	-title)
	    shift
	    title=$1
	    ;;
	-logScale)
	    shift
	    logScale=$1
	    ;;
	-path)
	    shift
	    path=$1
	    ;;
	-limit)
	    shift
	    limit=$1
	    ;;
    esac
    shift
done

if [ -z $logScale ]; then
    plot="unset logscale\n"
else
    plot="set logscale $logScale\n"
fi


plot=${plot}"set term pdf solid lw 1\n\
set output \"$output\"\n\
set grid\n\
set title \"$title\"\n\
set xlabel \"Number of elements in tree\"\n\
set ylabel \"Average Find path length\"\n\
set xtics rotate\n\
plot [:$limit]"

logFileNames=`echo $logFileNames | tr ',' ' '`

color=0
for logName in $logFileNames ; do
    ((color++))
    name=`echo $legendNames | cut -d',' -f $color`

    plot=${plot}"\"${path}/${logName}\" using (\$1/${scale}):2 w l title \"$name\" ls 1 lc $color,"
done

plot=`echo $plot | sed 's/.$//'`

plot="$plot \n\
set output\n\
set term pdf\n"

echo -e $plot | gnuplot
