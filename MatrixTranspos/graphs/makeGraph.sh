#!/bin/bash

output="graph.svg"
title="Average cost of ExtractMin on fibonacci heap"
logScale=""
path=$(pwd)
yrange="[:]"
color=0
scale=1
lw=2
lwChange=6
barsEvery=20
legend="outside right"
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
	-legend)
		shift
		legend=$1
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
set xlabel \"N: počet řádků / sloupců matice\"\n\
set ylabel \"Průměrný čas / počet načtených bloků na jedno prohození\"\n\
set xtics rotate\n\
set key $legend\n\
plot [:$limit]"

logFileNames=`echo $logFileNames | tr ',' ' '`

i=0
for logName in $logFileNames ; do
    ((color++))
	((i++))
    name=`echo $legendNames | cut -d',' -f $i`

	if [ "$i" -eq "$lwChange" ]; then
		color=1
		lw=4
	fi

    plot=${plot}"\"${path}/${logName}\" using (\$1/${scale}):2 w l title \"$name\" ls 1 lc $color lw ${lw},"

done

plot=`echo $plot | sed 's/.$//'`
echo -e $plot | gnuplot
