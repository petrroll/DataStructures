./makeGraph.sh -path "../data" \
-logFileNames "a_1.out a_2.out a_3.out a_4.out a_5.out a_6.out" \
-legendNames "10^1,10^2,10^3,10^4,10^5,10^6" \
-output "graph_1.pdf" \
-title "Double rotations"

./makeGraph.sh -path "../data" \
-logFileNames "s_1.out s_2.out s_3.out s_4.out s_5.out s_6.out" \
-legendNames "10^1,10^2,10^3,10^4,10^5,10^6" \
-output "graph_2.pdf" \
-title "Single rotations"

./makeGraph.sh -path "../data" \
-logFileNames "a_2.out s_2.out a_4.out s_4.out a_6.out s_6.out" \
-legendNames "Double: 10^2,Single: 10^2,Double: 10^4,Single: 10^4,Double: 10^6,Single: 10^6" \
-output "graph_3.pdf" \
-title "Double rotations vs. single rotations"

./makeGraph.sh -path "../data" \
-logFileNames "a_seq.out s_seq.out" \
-legendNames "Double rotations,Single rotations" \
-output "graph_4.pdf" \
-title "Double rotations vs. single rotations (sequential)"