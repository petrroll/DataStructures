./makeGraph.sh -path "../data" \
-logFileNames "real_naive.out real_adv_8x8.out" \
-legendNames "N,adv" \
-output "graph_1.pdf" \
-logScale x \
-title "ExtractMin standardní verze"

./makeGraph.sh -path "../data" \
-logFileNames "adv64x64.out adv64x1024.out adv64x4096.out adv512x512.out adv4096x64.out naive64x64.out naive64x1024.out naive64x4096.out naive512x512.out naive4096x64.out" \
-legendNames "adv64x64,adv64x1024,adv64x4096,adv512x512,adv4096x64,naive64x64,naive64x1024,naive64x4096,naive512x512,naive4096x64" \
-output "graph_2.pdf" \
-logScale x \
-title "ExtractMin standardní verze"

