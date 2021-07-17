
.PHONY: build

build_ui:
	cd ui && ng build --output-path ../core/ui-bin/

pack:
	packr -v -z

build: clean build_ui pack
	mkdir "bin"
	go build -v -a -o bin/simulator
	cp config.yml bin/config.yml

build_rpi: clean build_ui pack
	mkdir "bin"
	CGO_ENABLED=0 GOARCH=arm go build -v -a -o bin/simulator
	cp config.yml bin/config.yml

build_win: clean build_ui pack
	mkdir "bin"
	GOOS=windows go build -v -a -o bin/simulator.exe
	cp config.yml bin/config.yml

clean:
	rm -rv bin/
	packr clean