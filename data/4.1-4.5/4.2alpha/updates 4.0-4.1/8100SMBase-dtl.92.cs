"Change Set:		8100SMBase-dtl.92SMBase-dtl.92:Fix spelling error in error message"!!SMFileCache methodsFor: 'private' stamp: 'dtl 11/1/2009 12:50'!getStream: aDownloadable 	"Get the stream, either from the original url	or if that fails, from the server cache - unless	this is the actual server of course. :)	We also verify that the sha1sum is correct."	| stream |	[stream := aDownloadable downloadUrl asUrl retrieveContents contentStream binary.	(aDownloadable correctSha1sum: stream contents)		ifFalse: [self error: 'Incorrect SHA checksum of file from original URL']]		on: Exception do: [:ex |			Transcript show: 'Download from original url (', aDownloadable downloadUrl, ') failed with this exception: ', ex messageText;cr.			SMUtilities isServer				ifTrue: [^nil]				ifFalse: [					Transcript show: 'Trying server cache instead.'; cr.					[stream := (self cacheUrlFor: aDownloadable) asUrl retrieveContents contentStream binary.					(stream contents size = 21 and: [stream contents asString = 'SMFILEMISSINGONSERVER'])						ifTrue: [self error: 'File missing in server cache'].					(stream contents size = 24 and: [stream contents asString = 'SMRELEASENOTDOWNLOADABLE'])						ifTrue: [self error: 'Release not downloadable'].					(aDownloadable correctSha1sum: stream contents)						ifFalse: [self error: 'Incorrect SHA checksum of file from server']]							on: Exception do: [:ex2 | | msg |								msg := 'Download from server cache of ', aDownloadable printName, ' failed with this exception: ', ex2 messageText.								Transcript show: msg; cr.								self error: msg]]].	^ stream! !