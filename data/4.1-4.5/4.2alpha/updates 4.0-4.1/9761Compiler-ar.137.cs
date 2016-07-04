"Change Set:		9761Compiler-ar.137Compiler-ar.137:Underscore assignments and underscore selector preferences and implementation. The defaults are set to be compatible with past usage, i.e., allowUnderscoreAssignment is true and allowUnderscoreSelectors is false. Might consider changing this after 4.1."!Object subclass: #Scanner	instanceVariableNames: 'source mark hereChar aheadChar token tokenType currentComment buffer typeTable'	classVariableNames: 'AllowUnderscoreAssignments AllowUnderscoreSelectors TypeTable'	poolDictionaries: ''	category: 'Compiler-Kernel'!!Scanner methodsFor: 'private' stamp: 'ar 3/12/2010 15:51'!allowUnderscoreSelectors	"Query preference"	^self class prefAllowUnderscoreSelectors! !!Scanner class methodsFor: 'initialization' stamp: 'ar 3/12/2010 15:55'!initialize	| newTable |	newTable := Array new: 256 withAll: #xBinary. "default"	newTable atAll: #(9 10 12 13 32 ) put: #xDelimiter. "tab lf ff cr space"	newTable atAll: ($0 asciiValue to: $9 asciiValue) put: #xDigit.	1 to: 255		do: [:index |			(Character value: index) isLetter				ifTrue: [newTable at: index put: #xLetter]].	newTable at: 30 put: #doIt.	newTable at: $" asciiValue put: #xDoubleQuote.	newTable at: $# asciiValue put: #xLitQuote.	newTable at: $$ asciiValue put: #xDollar.	newTable at: $' asciiValue put: #xSingleQuote.	newTable at: $: asciiValue put: #xColon.	newTable at: $( asciiValue put: #leftParenthesis.	newTable at: $) asciiValue put: #rightParenthesis.	newTable at: $. asciiValue put: #period.	newTable at: $; asciiValue put: #semicolon.	newTable at: $[ asciiValue put: #leftBracket.	newTable at: $] asciiValue put: #rightBracket.	newTable at: ${ asciiValue put: #leftBrace.	newTable at: $} asciiValue put: #rightBrace.	newTable at: $^ asciiValue put: #upArrow.	newTable at: $_ asciiValue put: #xUnderscore.	newTable at: $| asciiValue put: #verticalBar.	TypeTable := newTable "bon voyage!!"	"Scanner initialize"! !!Parser methodsFor: 'private' stamp: 'ar 3/12/2010 16:10'!allowUnderscoreAssignments	"Query class + preference"	^encoder classEncoding allowUnderscoreAssignments		ifNil:[super allowUnderscoreAssignments]! !!Scanner methodsFor: 'private' stamp: 'ar 3/12/2010 15:51'!allowUnderscoreAssignments	"Query preference"	^self class prefAllowUnderscoreAssignments! !!Scanner class methodsFor: 'preferences' stamp: 'ar 3/12/2010 15:50'!prefAllowUnderscoreAssignments: aBool	"Accessor for the system-wide preference"	AllowUnderscoreAssignments := aBool! !!Scanner class methodsFor: 'preferences' stamp: 'ar 3/24/2010 01:19'!prefAllowUnderscoreAssignments	"Accessor for the system-wide preference"	<preference: 'Allow underscore assignments'		category: 'Compiler'		description: 'When true, underscore can be used as assignment operator'		type: #Boolean>	^AllowUnderscoreAssignments ifNil:[true]! !!Scanner class methodsFor: 'preferences' stamp: 'ar 3/12/2010 15:50'!prefAllowUnderscoreSelectors: aBool	"Accessor for the system-wide preference"	AllowUnderscoreSelectors := aBool! !!Scanner methodsFor: 'multi-character scans' stamp: 'ar 3/12/2010 16:10'!xUnderscore	self allowUnderscoreAssignments ifTrue:[ | type |		"Figure out if x _foo (no space between _ and foo) 		should be a selector or assignment"		(((type := self typeTableAt: aheadChar) == #xLetter			or:[type == #xDigit or:[type == #xUnderscore]]) 			and:[self allowUnderscoreSelectors]) ifFalse:[				self step.				tokenType := #leftArrow.				^token := #':='		].	].	self allowUnderscoreSelectors ifTrue:[^self xLetter].	^self xIllegal! !!Scanner methodsFor: 'multi-character scans' stamp: 'ar 3/12/2010 15:52'!xLetter	"Form a word or keyword."	| type |	buffer reset.	[(type := self typeTableAt: hereChar) == #xLetter		or: [type == #xDigit		or: [type == #xUnderscore and:[self allowUnderscoreSelectors]]]] whileTrue:			["open code step for speed"			buffer nextPut: hereChar.			hereChar := aheadChar.			aheadChar := source atEnd							ifTrue: [30 asCharacter "doit"]							ifFalse: [source next]].	tokenType := (type == #colon or: [type == #xColon and: [aheadChar ~~ $=]])					ifTrue: 						[buffer nextPut: self step.						"Allow any number of embedded colons in literal symbols"						[(self typeTableAt: hereChar) == #xColon] whileTrue:							[buffer nextPut: self step].						#keyword]					ifFalse: 						[type == #leftParenthesis 							ifTrue:								[buffer nextPut: self step; nextPut: $).								 #positionalMessage]							ifFalse:[#word]].	token := buffer contents! !!Scanner class methodsFor: 'preferences' stamp: 'ar 3/12/2010 15:50'!prefAllowUnderscoreSelectors	"Accessor for the system-wide preference"	<preference: 'Allow underscore selectors'		category: 'Compiler'		description: 'When true, underscore can be used in selectors and varibable names'		type: #Boolean>	^AllowUnderscoreSelectors ifNil:[false]! !!Parser methodsFor: 'private' stamp: 'ar 3/12/2010 15:55'!allowUnderscoreSelectors	"Query class + preference"	^encoder classEncoding allowUnderscoreSelectors		ifNil:[super allowUnderscoreSelectors]! !Scanner initialize!