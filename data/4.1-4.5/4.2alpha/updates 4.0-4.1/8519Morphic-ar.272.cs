"Change Set:		8519Morphic-ar.272Morphic-ar.272:CompiledMethodTrailer phase 2."!!TextEditor methodsFor: 'do-its' stamp: 'Igor.Stasenko 12/20/2009 07:27'!compileSelectionFor: anObject in: evalContext	| methodNode |	methodNode := [Compiler new		compileNoPattern: self selectionAsStream		in: anObject class		context: evalContext		notifying: self		ifFail: [^nil]]			on: OutOfScopeNotification			do: [:ex | ex resume: true].	^ methodNode generateWithTempNames! !