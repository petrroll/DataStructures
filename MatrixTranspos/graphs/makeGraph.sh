#!/bin/bash

output="graph.svg"
title="Average cost of ExtractMin on fibonacci heap"
logScale=""
path=$(pwd)
yrange="[:]"
color=0
scale=1
lw=1
lwChange=6
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
	-yrange)
	    shift
	    yrange=$1
	    ;;
	-color)
	    shift
	    color=$1
	    ;;
    esac
    shift
done

if [ -z $logScale ]; then
    plot="unset logscale\n"
	plot=${plot}"set yrange ${yrange}\n"
else
    plot="set logscale $logScale\n"
fi

plot=${plot}"set xrange [60:]\n"

plot=${plot}"set term pdf size 18cm,12cm solid lw 1\n\
set output \"$output\"\n\
set grid\n\
set title \"$title\"\n\
set xlabel \"Number of elements in the heap\"\n\
set ylabel \"Average cost of ExtractMin operation\"\n\
set xtics rotate\n\
set key outside right\n\
plot [:$limit]"

logFileNames=`echo $logFileNames | tr ',' ' '`

for logName in $logFileNames ; do
    ((color++))
    name=`echo $legendNames | cut -d',' -f $color`

	if [ "$color" -eq "$lwChange" ]; then
		color=1
		lw=3
	fi

    plot=${plot}"\"${path}/${logName}\" using (\$1/${scale}):2 w l title \"$name\" ls 1 lc $color lw ${lw},"

done

plot=`echo $plot | sed 's/.$//'`
echo -e $plot | gnuplot
