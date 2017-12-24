./makeGraph.sh -path "../data" \
-logFileNames "linTable.out linNaive.out linMulti.out cuckTable.out cuckMulti.out" \
-legendNames "linTable.out,linNaive.out,linMulti.out,cuckTable.out,cuckMulti.out" \
-output "graph_1.pdf" \
-legend "left" \
-yrange [0:20] \
-title "..."

./makeGraph.sh -path "../data" \
-logFileNames "linTable_time.out linNaive_time.out linMulti_time.out cuckTable_time.out cuckMulti_time.out" \
-legendNames "linTable.out,linNaive.out,linMulti.out,cuckTable.out,cuckMulti.out" \
-output "graph_2.pdf" \
-legend "left" \
-yrange [0:0.001] \
-title "..."

./makeGraph.sh -path "../data" \
-logFileNames "avg_linMultishift.out dec_linMultishift.out max_linMultishift.out med_linMultishift.out min_linMultishift.out" \
-legendNames "avg_linMultishift.outm,dec_linMultishift.out,max_linMultishift.out,med_linMultishift.out,min_linMultishift.out" \
-output "graph_3.pdf" \
-legend "left" \
-yrange [0:1000] \
-title "Multishift"


./makeGraph.sh -path "../data" \
-logFileNames "avg_linTable.out dec_linTable.out max_linTable.out med_linTable.out min_linTable.out" \
-legendNames "avg_linMultishift.outm,dec_linMultishift.out,max_linMultishift.out,med_linMultishift.out,min_linMultishift.out" \
-output "graph_4.pdf" \
-legend "left" \
-yrange [0:1000] \
-title "Table"