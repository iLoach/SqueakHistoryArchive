"Change Set:		7868Collections-ar.150Collections-ar.150:Merging Collections-ul.149:- removed zero check from WeakSet >> #like:Collections-ar.147:Merging Collections-ul.136- primitive 60 won't help with Array >> #at:ifAbsent: because it accepts only one parameter, so changed SequenceableCollection >> #at:ifAbsent: to get some improvement (assumes that indexes greater than the upper bound are more common than indexes smaller than the lower bound)Collections-ul.149:- removed zero check from WeakSet >> #like:Collections-ul.148:- replaced #whileFalse: with #whileFalse in #scanFor: and #scanForEmptySlotFor: implementations (both are inlined)- don't share associatinos with the new Dictionary in Dictionary >> #select:- avoid growing in Dictionary >> #collect:- removed temporary from Set >> #like:"!!IdentitySet methodsFor: 'private' stamp: 'ul 9/30/2009 14:21'!scanFor: anObject	"Scan the key array for the first slot containing either a nil (indicating an empty slot) or an element that matches anObject. Answer the index of that slot or raise an error if no slot is found. This method will be overridden in various subclasses that have different interpretations for matching elements."	| index start hash |	array size > 4096		ifTrue: [ hash := anObject identityHash * (array size // 4096) ]		ifFalse: [ hash := anObject identityHash ].	index := start := hash \\ array size + 1.	[ 		| element |		((element := array at: index) == nil or: [ element == anObject ])			ifTrue: [ ^index ].		(index := index \\ array size + 1) = start ] whileFalse.	self errorNoFreeSpace! !!Dictionary methodsFor: 'enumerating' stamp: 'ul 9/30/2009 14:14'!collect: aBlock 	"Evaluate aBlock with each of my values as the argument.  Collect the resulting values into a collection that is like me. Answer with the new collection."		| newCollection |	newCollection := self species new: self size.	self associationsDo: [ :each |		newCollection at: each key put: (aBlock value: each value) ].	^newCollection! !!IdentityDictionary methodsFor: 'private' stamp: 'ul 9/30/2009 14:21'!scanFor: anObject	"Scan the key array for the first slot containing either a nil (indicating an empty slot) or an element that matches anObject. Answer the index of that slot or raise an error if no slot is found. This method will be overridden in various subclasses that have different interpretations for matching elements."	| index start hash |	array size > 4096		ifTrue: [ hash := anObject identityHash * (array size // 4096) ]		ifFalse: [ hash := anObject identityHash ].	index := start := hash \\ array size + 1.	[ 		| element |		((element := array at: index) == nil or: [ element key == anObject ])			ifTrue: [ ^index ].		(index := index \\ array size + 1) = start ] whileFalse.	self errorNoFreeSpace! !!Dictionary methodsFor: 'enumerating' stamp: 'ul 9/30/2009 14:17'!select: aBlock 	"Evaluate aBlock with each of my values as the argument. Collect into a new dictionary, only those associations for which aBlock evaluates to true."	| newCollection |	newCollection := self species new.	self associationsDo: [ :each |		(aBlock value: each value) ifTrue: [			newCollection add: each copy ] ].	^newCollection! !!KeyedIdentitySet methodsFor: 'private' stamp: 'ul 9/30/2009 14:21'!scanFor: anObject	"Scan the key array for the first slot containing either a nil (indicating an empty slot) or an element that matches anObject. Answer the index of that slot or raise an error if no slot is found. This method will be overridden in various subclasses that have different interpretations for matching elements."	| index start hash |	array size > 4096		ifTrue: [ hash := anObject identityHash * (array size // 4096) ]		ifFalse: [ hash := anObject identityHash ].	index := start := hash \\ array size + 1.	[ 		| element |		((element := array at: index) == nil or: [ (keyBlock value: element) == anObject ])			ifTrue: [ ^index ].		(index := index \\ array size + 1) = start ] whileFalse.	self errorNoFreeSpace! !!KeyedSet methodsFor: 'private' stamp: 'ul 9/30/2009 14:21'!scanFor: anObject	"Scan the key array for the first slot containing either a nil (indicating an empty slot) or an element that matches anObject. Answer the index of that slot or raise an error if no slot is found. This method will be overridden in various subclasses that have different interpretations for matching elements."	| index start |	index := start := anObject hash \\ array size + 1.	[ 		| element |		((element := array at: index) == nil or: [ (keyBlock value: element) = anObject ])			ifTrue: [ ^index ].		(index := index \\ array size + 1) = start ] whileFalse.	self errorNoFreeSpace! !!WeakIdentityKeyDictionary methodsFor: 'private' stamp: 'ul 9/30/2009 14:20'!scanForEmptySlotFor: anObject	"Scan the key array for the first slot containing an empty slot (indicated by a nil). Answer the index of that slot. This method will be overridden in various subclasses that have different interpretations for matching elements."		| index start hash |	array size > 4096		ifTrue: [ hash := anObject identityHash * (array size // 4096) ]		ifFalse: [ hash := anObject identityHash ].	index := start := hash \\ array size + 1.	[ 		(array at: index) ifNil: [ ^index ].		(index := index \\ array size + 1) = start ] whileFalse.	self errorNoFreeSpace! !!SequenceableCollection methodsFor: 'accessing' stamp: 'ul 9/17/2009 06:00'!at: index ifAbsent: exceptionBlock 	"Answer the element at my position index. If I do not contain an element 	at index, answer the result of evaluating the argument, exceptionBlock."	(index <= self size  and: [ 1 <= index ]) ifTrue: [ ^self at: index ].	^exceptionBlock value! !!Set methodsFor: 'private' stamp: 'ul 9/30/2009 14:19'!scanForEmptySlotFor: anObject	"Scan the key array for the first slot containing an empty slot (indicated by a nil). Answer the index of that slot. This method will be overridden in various subclasses that have different interpretations for matching elements."		| index start |	index := start := anObject hash \\ array size + 1.	[ 		(array at: index) ifNil: [ ^index ].		(index := index \\ array size + 1) = start ] whileFalse.	self errorNoFreeSpace! !!Set methodsFor: 'accessing' stamp: 'ul 9/30/2009 14:33'!like: anObject	"Answer an object in the receiver that is equal to anObject,	nil if no such object is found. Relies heavily on hash properties"	^array at: (self scanFor: anObject)! !!PluggableDictionary methodsFor: 'private' stamp: 'ul 9/30/2009 14:19'!scanForEmptySlotFor: anObject	"Scan the key array for the first slot containing an empty slot (indicated by a nil). Answer the index of that slot. This method will be overridden in various subclasses that have different interpretations for matching elements."		| index start |	index := start := (hashBlock		ifNil: [ anObject hash ]		ifNotNil: [ hashBlock value: anObject ]) \\ array size + 1.	[ 		(array at: index) ifNil: [ ^index ].		(index := index \\ array size + 1) = start ] whileFalse.	self errorNoFreeSpace! !!PluggableSet methodsFor: 'private' stamp: 'ul 9/30/2009 14:19'!scanForEmptySlotFor: anObject	"Scan the key array for the first slot containing an empty slot (indicated by a nil). Answer the index of that slot. This method will be overridden in various subclasses that have different interpretations for matching elements."		| index start |	index := start := (hashBlock		ifNil: [ anObject hash ]		ifNotNil: [ hashBlock value: anObject ]) \\ array size + 1.	[ 		(array at: index) ifNil: [ ^index ].		(index := index \\ array size + 1) = start ] whileFalse.	self errorNoFreeSpace! !!Set methodsFor: 'private' stamp: 'ul 9/30/2009 14:23'!scanFor: anObject	"Scan the key array for the first slot containing either a nil (indicating an empty slot) or an element that matches anObject. Answer the index of that slot or raise an error if no slot is found. This method will be overridden in various subclasses that have different interpretations for matching elements."	| index start |	index := start := anObject hash \\ array size + 1.	[ 		| element |		((element := array at: index) == nil or: [ element = anObject ])			ifTrue: [ ^index ].		(index := index \\ array size + 1) = start ] whileFalse.	self errorNoFreeSpace! !!WeakSet methodsFor: 'private' stamp: 'ul 9/30/2009 14:24'!scanFor: anObject	"Scan the key array for the first slot containing either flag (indicating an empty slot) or an element that matches anObject. Answer the index of that slot or raise an error if no slot is found. This method will be overridden in various subclasses that have different interpretations for matching elements."	| index start |	index := start := anObject hash \\ array size + 1.	[ 		| element |		((element := array at: index) == flag or: [ element = anObject ])			ifTrue: [ ^index ].		(index := index \\ array size + 1) = start ] whileFalse.	self errorNoFreeSpace! !!PluggableDictionary methodsFor: 'private' stamp: 'ul 9/30/2009 14:23'!scanFor: anObject 	"Scan the key array for the first slot containing either a nil (indicating an empty slot) or an element that matches anObject. Answer the index of that slot or raise an error if no slot is found. This method will be overridden in various subclasses that have different interpretations for matching elements."		| index start |	index := start := (hashBlock		ifNil: [ anObject hash ]		ifNotNil: [ hashBlock value: anObject ]) \\ array size + 1.	[ 		| element |		((element := array at: index) == nil or: [			equalBlock				ifNil: [ element key = anObject ]				ifNotNil: [ equalBlock value: element key value: anObject ] ])			ifTrue: [ ^index ].		(index := index \\ array size + 1) = start ] whileFalse.	self errorNoFreeSpace! !!PluggableSet methodsFor: 'private' stamp: 'ul 9/30/2009 14:23'!scanFor: anObject 	"Scan the key array for the first slot containing either a nil (indicating an empty slot) or an element that matches anObject. Answer the index of that slot or raise an error if no slot is found. This method will be overridden in various subclasses that have different interpretations for matching elements."		| index start |	index := start := (hashBlock		ifNil: [ anObject hash ]		ifNotNil: [ hashBlock value: anObject ]) \\ array size + 1.	[ 		| element |		((element := array at: index) == nil or: [			equalBlock				ifNil: [ element = anObject ]				ifNotNil: [ equalBlock value: element value: anObject ] ])			ifTrue: [ ^index ].		(index := index \\ array size + 1) = start ] whileFalse.	self errorNoFreeSpace! !!Dictionary methodsFor: 'private' stamp: 'ul 9/30/2009 14:21'!scanFor: anObject	"Scan the key array for the first slot containing either a nil (indicating an empty slot) or an element that matches anObject. Answer the index of that slot or raise an error if no slot is found. This method will be overridden in various subclasses that have different interpretations for matching elements."	| index start |	index := start := anObject hash \\ array size + 1.	[ 		| element |		((element := array at: index) == nil or: [ element key = anObject ])			ifTrue: [ ^index ].		(index := index \\ array size + 1) = start ] whileFalse.	self errorNoFreeSpace! !!IdentitySet methodsFor: 'private' stamp: 'ul 9/30/2009 14:19'!scanForEmptySlotFor: anObject	"Scan the key array for the first slot containing an empty slot (indicated by a nil). Answer the index of that slot. This method will be overridden in various subclasses that have different interpretations for matching elements."		| index start hash |	array size > 4096		ifTrue: [ hash := anObject identityHash * (array size // 4096) ]		ifFalse: [ hash := anObject identityHash ].	index := start := hash \\ array size + 1.	[ 		(array at: index) ifNil: [ ^index ].		(index := index \\ array size + 1) = start ] whileFalse.	self errorNoFreeSpace! !!IdentityDictionary methodsFor: 'private' stamp: 'ul 9/30/2009 14:19'!scanForEmptySlotFor: anObject	"Scan the key array for the first slot containing an empty slot (indicated by a nil). Answer the index of that slot. This method will be overridden in various subclasses that have different interpretations for matching elements."		| index start hash |	array size > 4096		ifTrue: [ hash := anObject identityHash * (array size // 4096) ]		ifFalse: [ hash := anObject identityHash ].	index := start := hash \\ array size + 1.	[ 		(array at: index) ifNil: [ ^index ].		(index := index \\ array size + 1) = start ] whileFalse.	self errorNoFreeSpace! !!WeakSet methodsFor: 'private' stamp: 'ul 9/30/2009 14:20'!scanForEmptySlotFor: anObject	"Scan the key array for the first slot containing an empty slot (indicated by flag or a nil). Answer the index of that slot. This method will be overridden in various subclasses that have different interpretations for matching elements."		| index start |	index := start := anObject hash \\ array size + 1.	[ 		| element |		((element := array at: index) == flag or: [ element == nil ]) ifTrue: [ ^index ].		(index := index \\ array size + 1) = start ] whileFalse.	self errorNoFreeSpace! !!WeakIdentityKeyDictionary methodsFor: 'private' stamp: 'ul 9/30/2009 14:23'!scanFor: anObject	"Scan the key array for the first slot containing either a nil (indicating an empty slot) or an element that matches anObject. Answer the index of that slot or raise an error if no slot is found. This method will be overridden in various subclasses that have different interpretations for matching elements."	| index start hash |	array size > 4096		ifTrue: [ hash := anObject identityHash * (array size // 4096) ]		ifFalse: [ hash := anObject identityHash ].	index := start := hash \\ array size + 1.	[ 		| element |		((element := array at: index) == nil or: [ element key == anObject ])			ifTrue: [ ^index ].		(index := index \\ array size + 1) = start ] whileFalse.	self errorNoFreeSpace! !!WeakSet methodsFor: 'public' stamp: 'ul 10/1/2009 03:20'!like: anObject	"Answer an object in the receiver that is equal to anObject,	nil if no such object is found. Relies heavily on hash properties"	| element |	^(element  := array at: (self scanFor: anObject)) == flag		ifFalse: [ element ]! !!KeyedIdentitySet methodsFor: 'private' stamp: 'ul 9/30/2009 14:19'!scanForEmptySlotFor: anObject	"Scan the key array for the first slot containing an empty slot (indicated by a nil). Answer the index of that slot. This method will be overridden in various subclasses that have different interpretations for matching elements."		| index start hash |	array size > 4096		ifTrue: [ hash := anObject identityHash * (array size // 4096) ]		ifFalse: [ hash := anObject identityHash ].	index := start := hash \\ array size + 1.	[ 		(array at: index) ifNil: [ ^index ].		(index := index \\ array size + 1) = start ] whileFalse.	self errorNoFreeSpace! !