"Change Set:		9846Kernel-jcg.439Kernel-jcg.439:Fix Promise to address failing test added in KernelTests-jcg.146.Kernel-ar.436:Fix failing traits condenseChanges test. Since we do not share compiled methods, moving *all* trait methods (instead of just 'local' ones) is CORRECT behavior.Kernel-ar.437:Fixes needed for new condensing method:* Allow class comments to be moved to the sources file* Make CompiledMethod>>setSourcePointer: less expensive by avoiding #become: if at all possibleKernel-jcg.438:Fix embarrassing bug in Promise>>resolveWith:"!!ClassDescription methodsFor: 'fileIn/Out' stamp: 'ar 3/30/2010 23:54'!putClassCommentToCondensedChangesFile: aFileStream	"Called when condensing changes.  If the receiver has a class comment, and if that class comment does not reside in the .sources file, then write it to the given filestream, with the resulting RemoteString being reachable from the source file #2.  Note that any existing backpointer into the .sources file is lost by this process -- a situation that maybe should be fixed someday."	^self moveClassCommentTo: aFileStream fileIndex: 2! !!Promise methodsFor: 'resolving' stamp: 'jcg 4/5/2010 00:28'!resolveWith: arg	"Resolve this promise"	mutex critical: [		isResolved ifTrue: [self error: 'Promise was already resolved'].		value := arg.		isResolved := true.		resolvers ifNotNil: [resolvers do: [:r | self evaluateResolver: r]].	].! !!Promise methodsFor: 'waiting' stamp: 'jcg 4/6/2010 01:36'!waitTimeoutMSecs: msecs	"Wait for at most the given number of milliseconds for this promise to resolve. Answer true if it is resolved, false otherwise."	| sema delay |	sema := Semaphore new.	self whenResolved: [sema signal].	delay := Delay timeoutSemaphore: sema afterMSecs: msecs.	[sema wait] ensure: [delay unschedule].	^isResolved! !!ClassDescription methodsFor: 'fileIn/Out' stamp: 'ar 3/30/2010 23:53'!moveClassCommentTo: aFileStream fileIndex: newFileIndex	"Called when condensing changes.  If the receiver has a class comment, and if that class comment does not reside in the .sources file, then write it to the given filestream, with the resulting RemoteString being reachable from the source file fileIndex.  Note that any existing backpointer into the .sources file is lost by this process -- a situation that maybe should be fixed someday."	| header aStamp aCommentRemoteStr |	self isMeta ifTrue: [^ self].  "bulletproofing only"	((aCommentRemoteStr := self organization commentRemoteStr) isNil or:		[aCommentRemoteStr sourceFileNumber == 1]) ifTrue: [^ self].	aFileStream cr; nextPut: $!!.	header := String streamContents: [:strm | strm nextPutAll: self name;		nextPutAll: ' commentStamp: '.		(aStamp := self organization commentStamp ifNil: ['<historical>']) storeOn: strm.		strm nextPutAll: ' prior: 0'].	aFileStream nextChunkPut: header.	aFileStream cr.	self organization classComment: (RemoteString newString: self organization classComment onFileNumber: newFileIndex toFile: aFileStream) stamp: aStamp! !!CompiledMethod methodsFor: 'source code management' stamp: 'ar 3/31/2010 22:50'!setSourcePointer: srcPointer	"We can't change the trailer of existing method, since	it could have completely different format. Therefore we need to	generate a copy with new trailer, containing an scrPointer, and then	#become it"	| trailer copy start |	trailer := CompiledMethodTrailer new sourcePointer: srcPointer.	copy := self copyWithTrailerBytes: trailer.	"ar 3/31/2010: Be a bit more clever since #become: is slow.	If the old and the new trailer have the same size, just replace it."	(self trailer class == trailer class and:[self size = copy size]) ifTrue:[		start := self endPC + 1.		self replaceFrom: start to: self size with: copy startingAt: start.	] ifFalse:[self becomeForward: copy].	^self "will be copy if #become was needed"! !!ClassDescription methodsFor: 'fileIn/Out' stamp: 'ar 3/29/2010 20:51'!moveChangesTo: newFile 	"Used in the process of condensing changes, this message requests that 	the source code of all methods of the receiver that have been changed 	should be moved to newFile."	| changes |	changes := self methodDict keys select: [:sel |		(self compiledMethodAt: sel) fileIndex > 1].	self		fileOutChangedMessages: changes		on: newFile		moveSource: true		toFile: 2! !