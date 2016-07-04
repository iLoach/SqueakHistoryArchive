"Change Set:		8325Collections-ul.231Collections-ul.231:Part 2 of KeyedIdentitySet hash change."!!KeyedIdentitySet methodsFor: 'private' stamp: 'ul 12/1/2009 04:33'!scanFor: anObject	"Scan the key array for the first slot containing either a nil (indicating an empty slot) or an element that matches anObject. Answer the index of that slot or raise an error if no slot is found. This method will be overridden in various subclasses that have different interpretations for matching elements."	| index start |	index := start := anObject scaledIdentityHash \\ array size + 1.	[ 		| element |		((element := array at: index) == nil or: [ (keyBlock value: element) == anObject ])			ifTrue: [ ^index ].		(index := index \\ array size + 1) = start ] whileFalse.	self errorNoFreeSpace! !