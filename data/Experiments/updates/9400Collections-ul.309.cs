'From Squeak3.11alpha of 13 February 2010 [latest update: #9483] on 9 March 2010 at 11:11:23 am'!!Set methodsFor: 'accessing' stamp: 'ul 2/19/2010 14:49'!like: anObject ifAbsent: aBlock	"Answer an object in the receiver that is equal to anObject,	or evaluate the block if not found. Relies heavily on hash properties"		^(array at: (self scanFor: anObject))		ifNil: [ aBlock value ]		ifNotNil: [ :element | element enclosedSetElement ]! !!KeyedSet methodsFor: 'accessing' stamp: 'ul 2/19/2010 15:35'!like: anObject	"Answer an object in the receiver that is equal to anObject,	nil if no such object is found. Relies heavily on hash properties"	^(array at: (self scanFor: (keyBlock value: anObject)))		ifNotNil: [ :element | element enclosedSetElement]! !!KeyedSet methodsFor: 'accessing' stamp: 'ul 2/19/2010 15:35'!like: anObject ifAbsent: aBlock	"Answer an object in the receiver that is equal to anObject,	or evaluate the block if not found. Relies heavily on hash properties"	^(array at: (self scanFor: (keyBlock value: anObject)))		ifNil: [ aBlock value ]		ifNotNil: [ :element | element enclosedSetElement ]! !!WeakKeyAssociation methodsFor: 'accessing' stamp: 'ul 2/17/2010 13:54'!key	^key ifNotNil: [ key at: 1 ]! !!WeakKeyDictionary methodsFor: 'finalization' stamp: 'ul 2/17/2010 15:44'!finalizeValues	"Remove and finalize all elements which have nil key"		|  cleanUpAfterRemove |	tally = 0 ifTrue: [ ^self ].	cleanUpAfterRemove := false.	1 to: array size do: [ :index |		(array at: index) 			ifNil: [ cleanUpAfterRemove := false ]			ifNotNil: [ :element |				element key					ifNil: [						finalizer ifNotNil: [ finalizer value: element value ].						array at: index put: nil.						tally := tally - 1.						cleanUpAfterRemove := true ]					ifNotNil: [ :key |						cleanUpAfterRemove ifTrue: [							| newIndex |							(newIndex := self scanFor: key) = index ifFalse: [								array 									at: newIndex put: element;									at: index put: nil ] ] ] ] ]! !!WeakKeyDictionary methodsFor: 'private' stamp: 'ul 2/17/2010 13:37'!fixCollisionsFrom: start	"The element at start has been removed and replaced by nil.	This method moves forward from there, relocating any entries	that had been placed below due to collisions with this one."	| element index |	index := start.	[ (element := array at: (index := index \\ array size + 1)) == nil ] whileFalse: [		element key			ifNil: [ 				finalizer ifNotNil: [ finalizer value: element value ].				array at: index put: nil.				tally := tally - 1 ]			ifNotNil: [ :key | "Don't let the key go away"				| newIndex |				(newIndex := self scanFor: key) = index ifFalse: [					array 						at: newIndex put: element;						at: index put: nil ] ] ]! !!WeakSet methodsFor: 'accessing' stamp: 'ul 2/19/2010 14:56'!like: anObject ifAbsent: aBlock	"Answer an object in the receiver that is equal to anObject,	or evaluate the block if not found. Relies heavily on hash properties"	| element |	((element  := array at: (self scanFor: anObject)) == flag or: [ element == nil ])		ifTrue: [ ^aBlock value ]		ifFalse: [ ^element enclosedSetElement ]! !