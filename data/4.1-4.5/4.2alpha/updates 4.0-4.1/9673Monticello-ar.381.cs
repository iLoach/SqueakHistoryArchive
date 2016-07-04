"Change Set:		9673Monticello-ar.381Monticello-ar.381:Add preference to make new version check before save optional. Also truncate newer version list to 150 characters to avoid extremely long messages."!MCTool subclass: #MCWorkingCopyBrowser	instanceVariableNames: 'workingCopy workingCopyWrapper repository defaults'	classVariableNames: 'CheckForNewerVersionsBeforeSave'	poolDictionaries: ''	category: 'Monticello-UI'!!MCWorkingCopyBrowser class methodsFor: 'preferences' stamp: 'ar 3/10/2010 21:12'!checkForNewerVersionsBeforeSave	"Preference accessor"	<preference: 'Check for new versions before save'		category: 'Monticello'		description: 'If true, MC will warn before committing to repositories that have possibly newer versions of the package being saved.'		type: #Boolean>	^CheckForNewerVersionsBeforeSave ifNil:[true]! !!MCWorkingCopyBrowser methodsFor: 'actions' stamp: 'ar 3/10/2010 21:14'!checkForNewerVersions	| newer |	newer := workingCopy possiblyNewerVersionsIn: self repository.	^ newer isEmpty or: [		self confirm: 'CAUTION!! These versions in the repository may be newer:', 			String cr, (newer asString truncateWithElipsisTo: 150), String cr,			'Do you really want to save this version?'].! !!MCWorkingCopyBrowser class methodsFor: 'preferences' stamp: 'ar 3/10/2010 21:12'!checkForNewerVersionsBeforeSave: aBool	"Sets the CheckForNewerVersionsBeforeSave preference"	CheckForNewerVersionsBeforeSave := aBool! !