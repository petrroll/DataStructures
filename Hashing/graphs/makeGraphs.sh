./makeGraph.sh -path "../data" \
-logFileNames "linTable.out linNaive.out linMulti.out cuckTable.out cuckMulti.out" \
-legendNames "Lineární: tabulková,Lineární: naivní modulo,Lineární: multiply-shift,Kukaččí: tabulková,Kukaččí: multiply-shift" \
-output "graph_rand_steps.pdf" \
-legend "left" \
-yrange [0:20] \
-title "Náhodný testovací případ: průměrný počet kroků vs. zaplnění." \
-xTitle "Zaplněnost hashovací tabulky o velikosti 2^24." \
-yTitle "Průměrný počet kroků operace insert (přes 2^24 / 100 prvků)."

./makeGraph.sh -path "../data" \
-logFileNames "linTable_time.out linNaive_time.out linMulti_time.out cuckTable_time.out cuckMulti_time.out" \
-legendNames "Lineární: tabulková,Lineární: naivní modulo,Lineární: multiply-shift,Kukaččí: tabulková,Kukaččí: multiply-shift" \
-output "graph_rand_time.pdf" \
-legend "left" \
-yrange [0:0.0015] \
-title "Náhodný testovací případ: průměrný čas na insert vs. zaplnění." \
-xTitle "Zaplněnost hashovací tabulky o velikosti 2^24." \
-yTitle "Průměrná doba operace insert (přes 2^24 / 100 prvků) [ms]."

./makeGraph.sh -path "../data" \
-logFileNames "max_linTable.out dec_linTable.out avg_linTable.out med_linTable.out min_linTable.out max_linMultishift.out dec_linMultishift.out avg_linMultishift.out med_linMultishift.out min_linMultishift.out" \
-legendNames "Tabulková: Maximum,Tabulková: 9. Decil,Tabulková: Průměr,Tabulková: Medián,Tabulková: Minimum,Multiply-shift: Maximum,Multiply-shift: 9. Decil,Multiply-shift: Průměr,Multiply-shift: Medián,Multiply-shift: Minimum" \
-output "graph_seq_both.pdf" \
-legend "left" \
-yrange [0:500] \
-title "Sekvenční test: průměrný počet kroků insertu vs. velikost tabulky." \
-xTitle "Velikost tabulky v bitech (|bins| = 2^i)" \
-yTitle "Průměrný počet kroků operace insert (při plnení od 0.89 do 0.91)."


./makeGraph.sh -path "../data" \
-logFileNames "max_linMultishift.out dec_linMultishift.out avg_linMultishift.out med_linMultishift.out min_linMultishift.out" \
-legendNames "Maximum,9. Decil,Průměr,Medián,Minimum" \
-output "graph_seq_multishift.pdf" \
-legend "left" \
-yrange [0:100] \
-title "Sekvenční test: průměrný počet kroků insertu vs. velikost tabulky pro multiply-shift." \
-xTitle "Velikost tabulky v bitech (|bins| = 2^i)" \
-yTitle "Průměrný počet kroků operace insert (při plnení od 0.89 do 0.91)."

./makeGraph.sh -path "../data" \
-logFileNames "max_linTable.out dec_linTable.out avg_linTable.out med_linTable.out min_linTable.out" \
-legendNames "Maximum,9. Decil,Průměr,Medián,Minimum" \
-output "graph_seq_table.pdf" \
-legend "left" \
-yrange [0:100] \
-title "Sekvenční test: průměrný počet kroků insertu vs. velikost tabulky pro tabulkový hash." \
-xTitle "Velikost tabulky v bitech (|bins| = 2^i)" \
-yTitle "Průměrný počet kroků operace insert (při plnení od 0.89 do 0.91)."

