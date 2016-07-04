"Change Set:		9359ST80-dtl.105ST80-dtl.105:Implement MVCProject>>showImage:named: to eliminate MVC/Morphic dependency in HTTPSocket.Update various method comments in FillInTheBlank to encourage use of UIManager default."!!FillInTheBlank class methodsFor: 'instance creation' stamp: 'dtl 2/12/2010 19:53'!request: queryString 	"Create an instance of me whose question is queryString. Invoke it 	centered at the cursor, and answer the string the user accepts. Answer 	the empty string if the user cancels."	"UIManager default request: 'Your name?'"	^ self		request: queryString		initialAnswer: ''		centerAt: (ActiveHand ifNil:[Sensor]) cursorPoint! !!FillInTheBlank class methodsFor: 'instance creation' stamp: 'dtl 2/12/2010 19:53'!requestPassword: queryString 	"Create an instance of me whose question is queryString. Invoke it centered	at the cursor, and answer the string the user accepts. Answer the empty 	string if the user cancels."	"UIManager default requestPassword: 'POP password'"	| model fillInView |	Smalltalk isMorphic 		ifTrue: [^self fillInTheBlankMorphClass requestPassword: queryString].	model := self new.	model contents: ''.	fillInView := self fillInTheBlankViewClass 				requestPassword: model				message: queryString				centerAt: Sensor cursorPoint				answerHeight: 40.	^model show: fillInView! !!FillInTheBlank class methodsFor: 'instance creation' stamp: 'dtl 2/12/2010 19:52'!request: queryString initialAnswer: defaultAnswer centerAt: aPoint 	"Create an instance of me whose question is queryString with the given	initial answer. Invoke it centered at the given point, and answer the	string the user accepts. Answer the empty string if the user cancels."	"UIManager default		request: 'Type something, then type CR.'		initialAnswer: 'yo ho ho!!'		centerAt: Display center"	| model fillInView |	Smalltalk isMorphic 		ifTrue: 			[^self fillInTheBlankMorphClass 				request: queryString				initialAnswer: defaultAnswer				centerAt: aPoint].	model := self new.	model contents: defaultAnswer.	fillInView := self fillInTheBlankViewClass 				on: model				message: queryString				centerAt: aPoint.	^model show: fillInView! !!MVCProject methodsFor: 'utilities' stamp: 'dtl 2/12/2010 20:41'!showImage: aForm named: imageName	"Show an image, possibly attached to the pointer for positioning"	FormView open: aForm named: imageName! !!FillInTheBlank class methodsFor: 'instance creation' stamp: 'dtl 2/12/2010 19:52'!request: queryString initialAnswer: defaultAnswer 	"Create an instance of me whose question is queryString with the given 	initial answer. Invoke it centered at the given point, and answer the 	string the user accepts. Answer the empty string if the user cancels."	"UIManager default 		request: 'What is your favorite color?' 		initialAnswer: 'red, no blue. Ahhh!!'"	^ self		request: queryString		initialAnswer: defaultAnswer		centerAt: (ActiveHand ifNil:[Sensor]) cursorPoint! !!FillInTheBlank class methodsFor: 'private' stamp: 'dtl 2/12/2010 19:36'!fillInTheBlankMorphClass	"By factoring out this class references, it becomes possible to discard 	morphic by simply removing this class.  All calls to this method needs	to be protected by 'Smalltalk isMorphic' tests."	^ Smalltalk		at: #FillInTheBlankMorph		ifAbsent: [self notify: 'Morphic class FillInTheBlankMorph not present']! !!FillInTheBlank class methodsFor: 'instance creation' stamp: 'dtl 2/12/2010 19:39'!multiLineRequest: queryString centerAt: aPoint initialAnswer: defaultAnswer answerHeight: answerHeight 	"Create a multi-line instance of me whose question is queryString with	the given initial answer. Invoke it centered at the given point, and	answer the string the user accepts.  Answer nil if the user cancels.  An	empty string returned means that the ussr cleared the editing area and	then hit 'accept'.  Because multiple lines are invited, we ask that the user	use the ENTER key, or (in morphic anyway) hit the 'accept' button, to 	submit; that way, the return key can be typed to move to the next line.	NOTE: The ENTER key does not work on Windows platforms."	"UIManager default		multiLineRequest:'Enter several lines; end input by acceptingor canceling via menu or press Alt+s/Alt+l'		centerAt: Display center		initialAnswer: 'Once upon a time...'		answerHeight: 200"	| model fillInView |	Smalltalk isMorphic 		ifTrue: 			[^self fillInTheBlankMorphClass 				request: queryString				initialAnswer: defaultAnswer				centerAt: aPoint				inWorld: self currentWorld				onCancelReturn: nil				acceptOnCR: false].	model := self new.	model contents: defaultAnswer.	model responseUponCancel: nil.	model acceptOnCR: false.	fillInView := self fillInTheBlankViewClass 				multiLineOn: model				message: queryString				centerAt: aPoint				answerHeight: answerHeight.	^model show: fillInView! !