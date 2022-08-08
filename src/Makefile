scf4: nim.exe
	./nim.exe


game.o: game.c
	gcc game.c -c -fPIC -lraylib -lm -I.


nim.exe: main.nim game.o bindings.nim
	nim c \
		-o:nim.exe \
		--threads:on \
		--passL:game.o \
		--passL:"-lraylib" \
		-p:$(PARARULES) \
		-p:/lkj/ \
		main.nim

clean:
	rm *.exe *.o
