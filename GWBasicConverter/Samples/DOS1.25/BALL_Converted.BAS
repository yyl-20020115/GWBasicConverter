940	REM The IBM Personal Computer Ball
950	REM Version 1.10 (C)Copyright IBM Corp 1981, 1982
960	REM Licensed Material - Program Property of IBM
975	DEF SEG: POKE 106,0
980	SAMPLES$="NO"
990	GOTO 1010 
1000	SAMPLES$="YES"
1010	KEY OFF:SCREEN 0,1:COLOR 15,0,0:WIDTH 40:CLS:LOCATE 5,19:PRINT "IBM"
1020	LOCATE 7,12,0:PRINT "Personal Computer"
1030	COLOR 10,0:LOCATE 10,9,0:PRINT CHR$(213)+STRING$(21,205)+CHR$(184)
1040	LOCATE 11,9,0:PRINT CHR$(179)+"        BALL         "+CHR$(179)
1050	LOCATE 12,9,0:PRINT CHR$(179)+STRING$(21,32)+CHR$(179)
1060	LOCATE 13,9,0:PRINT CHR$(179)+"    Version 1.10     "+CHR$(179)
1070	LOCATE 14,9,0:PRINT CHR$(212)+STRING$(21,205)+CHR$(190)
1080	COLOR 15,0:LOCATE 17,4,0:PRINT "(C) Copyright IBM Corp 1981, 1982
1090	COLOR 14,0:LOCATE 23,7,0:PRINT "Press space bar to continue"
1100	IF INKEY$<>"" THEN GOTO 1100 
1110	CMD$ = INKEY$
1120	IF CMD$ = "" THEN GOTO 1110 
1130	IF CMD$ = CHR$(27) THEN GOTO 1298 
1140	IF CMD$ = " " THEN GOTO 1160 
1150	GOTO 1110 
1160	DEF SEG=0
1170	IF (PEEK(&H410) AND &H30)<>&H30 THEN DEF SEG:GOTO 1291 
1180	WIDTH 80:CLS:LOCATE 3,1
1190	PRINT "HOLD IT!"
1200	PRINT "YOU'RE NOT USING THE COLOR/GRAPHICS MONITOR ADAPTER!"
1210	PRINT "THIS PROGRAM USES GRAPHICS AND REQUIRES THAT ADAPTER."
1220	PRINT "PRESS THE SPACE BAR TO CONTINUE."
1230	DEF SEG
1240	IF INKEY$<>"" THEN GOTO 1240 
1250	CMD$ = INKEY$
1260	IF CMD$ = "" THEN GOTO 1250 
1270	IF CMD$ = CHR$(27) THEN GOTO 1298 
1280	IF CMD$ = " " THEN GOTO 1298 
1290	GOTO 1250 
1291	KEY OFF
1292	ON ERROR GOTO 1295 
1293	PLAY "p16"
1294	GOTO 1300 
1295	COLOR 31,0,0
1296	PRINT "THIS PROGRAM REQUIRES ADVANCED BASIC -- USE COMMAND 'BASICA'":COLOR 15,0,0:FOR I=1 TO 9000:NEXT: RESUME 1298 
1298	ON ERROR GOTO 0 
1299	SCREEN 0,1:IF SAMPLES$="YES" THEN CHAIN"samples",1000 :ELSE COLOR 7,0,0:CLS:END
1300	REM
1410	PLAY "mfl32t067"
1420	DEFINT A-Z: DIM B(280): DIM A(150)
1430	CLS:SCREEN 1,0:COLOR 8,0:KEY OFF
1440	CIRCLE (160,100),10,2
1450	PAINT (160,100),2,2
1460	GET (150,90)-(170,110),A
1470	CLS:LINE (19,0)-(299,177),,B
1480	LINE (20,1)-(300,178),,B
1490	LOCATE 24,11:PRINT" Press ESC to exit ";
1500	FOR X=20 TO 280 STEP 4
1510	B(X)=159-CINT(ABS(SIN(X*0.078540)*X)\2)
1520	NEXT: L=7: Y2=150
1530	LINE (223,3)-(253,175),3,BF
1540	LINE (191,3)-(222,175),2,BF
1550	LINE (160,3)-(190,175),1,BF
1560	FOR D=0 TO 1
1570	S=20+D*260:E=280-D*260
1580	FOR X=S TO E STEP 4-8*D
1590	Y=B(X): N=(170-Y2)\5:PLAY "L=L;T255;N=N;": IF F THEN PUT (X2,Y2),A
1600	PUT (X,Y),A: X2=X: Y2=Y: F=-1: NEXT
1610	L=(L+7) MOD 64:NEXT
1615	A$=INKEY$:IF A$=CHR$(27) THEN 1298 
1620	POKE 106,0:IF A$<>"" THEN 1615  :ELSE GOTO 1560 
