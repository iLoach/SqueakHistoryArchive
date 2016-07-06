'From Squeak3.11alpha of 13 February 2010 [latest update: #9483] on 9 March 2010 at 11:11:23 am'!!SocketStream methodsFor: 'stream in' stamp: 'ar 2/23/2010 13:06'!next: n into: aCollection	"Read n objects into the given collection.	Return aCollection or a partial copy if less than	n elements have been read."	^self next: n into: aCollection startingAt: 1! !!SocketStream methodsFor: 'stream in' stamp: 'ar 2/23/2010 13:05'!next: anInteger into: aCollection startingAt: startIndex	"Read n objects into the given collection. 	Return aCollection or a partial copy if less than	n elements have been read."	"Implementation note: This method DOES signal timeout if not 	enough elements are received. It does NOT signal	ConnectionClosed as closing the connection is the only way by	which partial data can be read."	| start amount |	[self receiveData: anInteger] on: ConnectionClosed do:[:ex| ex return].	"Inlined version of nextInBuffer: to avoid copying the contents"	amount := anInteger min: (inNextToWrite - lastRead - 1).	start := lastRead + 1.	lastRead := lastRead + amount.	aCollection 		replaceFrom: startIndex 		to: startIndex + amount-1 		with: inBuffer 		startingAt: start.	^amount < anInteger 		ifTrue:[aCollection copyFrom: 1 to:  startIndex + amount-1]		ifFalse:[aCollection]! !!SocketStream methodsFor: 'stream in' stamp: 'ar 2/23/2010 13:06'!nextInto: aCollection	"Read the next elements of the receiver into aCollection.	Return aCollection or a partial copy if less than aCollection	size elements have been read."	^self next: aCollection size into: aCollection startingAt: 1.! !!SocketStream methodsFor: 'stream in' stamp: 'ar 2/23/2010 13:06'!nextInto: aCollection startingAt: startIndex	"Read the next elements of the receiver into aCollection.	Return aCollection or a partial copy if less than aCollection	size elements have been read."	^self next: (aCollection size - startIndex+1) into: aCollection startingAt: startIndex.! !