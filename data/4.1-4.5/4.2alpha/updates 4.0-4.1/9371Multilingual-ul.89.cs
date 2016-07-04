"Change Set:		9371Multilingual-ul.89Multilingual-ul.89:- removed EUCTextConverter >> #saveStateOf: and EUCTextConverter >> #restoreStateOf:with:, because they were same as in super- more chunk reading capabilities for TextConverter for better performance: #nextChunkTextFromStream: and #parseLanguageTagFor:fromStream: (part 1)"!!TextConverter methodsFor: 'fileIn/Out' stamp: 'ul 2/7/2010 04:39'!nextChunkTextFromStream: input	"Deliver the next chunk as a Text.  Decode the following ]style[ chunk if present.  Position at start of next real chunk."		| chunk state runs |	chunk := self nextChunkFromStream: input.	state := self saveStateOf: input.	(input skipSeparatorsAndPeekNext == $] and: [		(input next: 7) = ']style[' ])			ifTrue: [				runs := RunArray scanFrom: (self nextChunkFromStream: input) readStream ]			ifFalse: [				self restoreStateOf: input with: state.				runs := RunArray new: chunk size withAll: #() ].	^Text string: chunk runs: runs! !!TextConverter methodsFor: 'fileIn/Out' stamp: 'ul 2/7/2010 04:24'!parseLangTagFor: aString fromStream: stream	| state |	state := self saveStateOf: stream.	"Test for ]lang[ tag"	(stream skipSeparatorsAndPeekNext == $] and: [		(stream next: 6) = ']lang[' ]) ifTrue: [			^stream				decodeString: aString				andRuns: (self nextChunkFromStream: stream) ].	"no tag"	self restoreStateOf: stream with: state.	^aString			! !!UTF8TextConverter methodsFor: 'fileIn/Out' stamp: 'ul 2/7/2010 04:41'!nextChunkTextFromStream: input	"Deliver the next chunk as a Text.  Decode the following ]style[ chunk if present.  Position at start of next real chunk."		| chunk state runs |	chunk := self nextChunkFromStream: input.	state := self saveStateOf: input.	(input skipSeparatorsAndPeekNext == $] and: [		(input basicNext: 7) = ']style[' ])			ifTrue: [				runs := RunArray scanFrom: (self nextChunkFromStream: input) readStream ]			ifFalse: [				self restoreStateOf: input with: state.				runs := RunArray new: chunk size withAll: #() ].	^Text string: chunk runs: runs! !!UTF8TextConverter methodsFor: 'fileIn/Out' stamp: 'ul 2/7/2010 04:22'!parseLangTagFor: aString fromStream: stream	| state |	state := self saveStateOf: stream.	"Test for ]lang[ tag"	(stream skipSeparatorsAndPeekNext == $] and: [		(stream basicNext: 6) = ']lang[' ]) ifTrue: [			^stream				decodeString: aString				andRuns: (self nextChunkFromStream: stream) ].	"no tag"	self restoreStateOf: stream with: state.	^aString! !!TextConverter methodsFor: 'fileIn/Out' stamp: 'ul 2/7/2010 04:25'!nextChunkFromStream: input	"Answer the contents of input, up to the next terminator character. Doubled terminators indicate an embedded terminator character."		input skipSeparators.	^self		parseLangTagFor: (			String new: 1000 streamContents: [ :output |				| character state |				[ 					(character := self nextFromStream: input) == nil or: [ 						character == $!! and: [ 							state := self saveStateOf: input.							(self nextFromStream: input) ~~ $!! ] ] ] 					whileFalse: [ output nextPut: character ].				character ifNotNil: [ 					self restoreStateOf: input with: state ] ])		fromStream: input! !!UTF8TextConverter methodsFor: 'fileIn/Out' stamp: 'ul 2/7/2010 04:23'!nextChunkFromStream: input	"Answer the contents of input, up to the next terminator character. Doubled terminators indicate an embedded terminator character."		input skipSeparators.	^self 		parseLangTagFor: (			String new: 1000 streamContents: [ :stream |				[					stream nextPutAll: (input basicUpTo: $!!).					input basicNext == $!! ]						whileTrue: [ 							stream nextPut: $!! ].				input atEnd ifFalse: [ input skip: -1 ] ]) utf8ToSqueak		fromStream: input! !EUCTextConverter removeSelector: #restoreStateOf:with:!EUCTextConverter removeSelector: #saveStateOf:!