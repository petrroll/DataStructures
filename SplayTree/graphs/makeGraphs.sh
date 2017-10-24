./graph.sh -path "../data" \
-logFileNames "a_1.out a_2.out a_3.out a_4.out a_5.out a_6.out" \
-legendNames "10^1,10^2,10^3,10^4,10^5,10^6" \
-output "graph_1.svg" \
-title "Double rotations"

./graph.sh -path "../data" \
-logFileNames "s_1.out s_2.out s_3.out s_4.out s_5.out s_6.out" \
-legendNames "10^1,10^2,10^3,10^4,10^5,10^6" \
-output "graph_2.svg" \
-title "Single rotations"

./graph.sh -path "../data" \
-logFileNames "a_2.out s_2.out a_4.out s_4.out a_6.out s_6.out" \
-legendNames "Double: 10^2,Single: 10^2,Double: 10^4,Single: 10^4,Double: 10^6,Single: 10^6" \
-output "graph_3.svg" \
-title "Double rotations vs. single rotations"

./graph.sh -path "../data" \
-logFileNames "a_seq.out s_seq.out" \
-legendNames "Double rotations,Single rotations" \
-output "graph_4.svg" \
-title "Double rotations vs. single rotations (sequential)"