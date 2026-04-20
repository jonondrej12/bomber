# Bomberman (Unity)

## O projektu
Tento projekt je inspirován hrou Bomberman (DynaBlaster) vytvořený v Unity v jazyce C#. Hráč prochází generovaným bludištěm, pokládá bomby, ničí zdi a bojuje s nepřáteli nebo ostatními hráči.

Hra obsahuje více herních režimů:
- singleplayer
- 2 hráči na jednom PC
- multiplayer přes síť (2 hráči)

## Technologie
- Unity
- C#
- Netcode (multiplayer)

## Herní princip
- Hráč se pohybuje v bludišti a pokládá bomby
- Bomby explodují do čtyř směrů a ničí zdi i nepřátele
- Ve zničitelných zdech lze najít power-upy nebo portál do další úrovňe
- Cílem je přežít a postoupit do další úrovně nebo porazit protihráče

## Hlavní herní systémy

### Generování levelu
Bludiště se generuje náhodně:
- náhodné zdi
- náhodné rozmístění nepřátel
- zvyšující se obtížnost v dalších levelech

### Hráč
- pohyb + pokládání bomb
- omezený počet bomb
- životy a restart levelu po smrti

### Bomby a exploze
- časovač 3 sekundy
- exploze do 4 směrů
- interakce se zdmi, hráči a nepřáteli

### Nepřátelé
- 2 typy (běžní a rychlí)
- náhodný pohyb po mapě
- reakce na překážky

## Multiplayer
- implementováno pomocí Netcode
- jeden hráč hostuje hru
- ostatní se připojují přes IP
- synchronizace pohybu a herního stavu

## Spuštění projektu
Z důvodu velikosti Unity projektu je repozitář omezen pouze na zdrojový kód a spustitelnou verzi hry.

Hru lze spustit přímo pomocí `.exe` souboru ve složce `build`.


