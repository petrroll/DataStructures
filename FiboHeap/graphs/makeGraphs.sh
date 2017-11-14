./makeGraph.sh -path "../data" \
-logFileNames "adv.b.out adv.r.out" \
-legendNames "Nevyvážený,Rovnoměrný" \
-output "graph_1.pdf" \
-logScale y \
-title "ExtractMin standardní verze"

./makeGraph.sh -path "../data" \
-logFileNames "adv.b.out" \
-legendNames "Nevyvážený" \
-output "graph_1_1.pdf" \
-yrange "[500:2500]" \
-title "ExtractMin standardní verze"

./makeGraph.sh -path "../data" \
-logFileNames "adv.r.out" \
-legendNames "Rovnoměrný" \
-output "graph_1_2.pdf" \
-yrange "[10:20]" \
-color 1 \
-title "ExtractMin standardní verze"

./makeGraph.sh -path "../data" \
-logFileNames "adv.x.out simp.x.out" \
-legendNames "Standardní,Naivní" \
-output "graph_2.pdf" \
-title "Standardní vs. naivní verze"

