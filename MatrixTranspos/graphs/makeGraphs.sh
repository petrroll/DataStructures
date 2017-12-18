./makeGraph.sh -path "../data" \
-logFileNames "real_naive.out real_adv_8x8.out" \
-legendNames "Naive,CacheObliviousStopAt8x8" \
-output "graph_1.pdf" \
-logScale "x" \
-legend "left" \
-title "Transpozice matic: Intel i5 2500K @4.2GHz (L1: 32KB Data 8w, L2: 256KB 8w, L3: 6MB 12w)"

./makeGraph.sh -path "../data" \
-logFileNames "adv64x64.out adv64x1024.out adv64x4096.out adv512x512.out adv4096x64.out naive64x64.out naive64x1024.out naive64x4096.out naive512x512.out naive4096x64.out" \
-legendNames "adv64x64,adv64x1024,adv64x4096,adv512x512,adv4096x64,naive64x64,naive64x1024,naive64x4096,naive512x512,naive4096x64" \
-output "graph_2.pdf" \
-logScale "x" \
-title "Transpozice matic: simultátor"

./makeGraph.sh -path "../data" \
-logFileNames "adv64x64.out adv64x1024.out adv64x4096.out adv512x512.out adv4096x64.out" \
-legendNames "adv64x64,adv64x1024,adv64x4096,adv512x512,adv4096x64" \
-output "graph_2_adv.pdf" \
-logScale "x" \
-title "Transpozice matic: simultátor (cache oblivious alg)"

./makeGraph.sh -path "../data" \
-logFileNames "naive64x64.out naive64x1024.out naive64x4096.out naive512x512.out naive4096x64.out" \
-legendNames "naive64x64,naive64x1024,naive64x4096,naive512x512,naive4096x64" \
-output "graph_2_naive.pdf" \
-logScale "x" \
-title "Transpozice matic: simultátor (naive alg)"

