mySettings = {
	previewAutoRefresh: false,
	previewParser: function (content) {
		var htmlContent = XBBCODE.process({
			text: content,
			addInLineBreaks: true
		}).html;

		return htmlContent.replace("[hr]", "<hr/>");
	},
	previewParserPath: "templates/preview.html",
	markupSet: [
		{ name: 'Bold', key: 'B', openWith: '[b]', closeWith: '[/b]' },
		{ name: 'Italic', key: 'I', openWith: '[i]', closeWith: '[/i]' },
		{ name: 'Underline', key: 'U', openWith: '[u]', closeWith: '[/u]' },
		{ name: 'Strike', key: 'S', openWith: '[s]', closeWith: '[/s]' },
		{ name: 'Sup', key: '.', openWith: '[sup]', closeWith: '[/sup]' },
		{ name: 'Sub', key: ',', openWith: '[sub]', closeWith: '[/sub]' },
		{ separator: '---------------' },
		{ name: 'Justify', openWith: '[align=justify]', closeWith: '[/align]' },
		{ name: 'Right', openWith: '[align=right]', closeWith: '[/align]' },
		{ name: 'Center', openWith: '[align=center]', closeWith: '[/align]' },
		{ name: 'Left', openWith: '[align=left]', closeWith: '[/align]' },
		{ separator: '---------------' },
		{ name: 'Picture', key: 'P', replaceWith: '[img][![Image Url:!:http://]!][/img]' },
		{ name: 'Link', key: 'L', openWith: '[url=[![Site Url:!:http://]!]]', closeWith: '[/url]' },
		{ name: 'Youtube', key: 'Y', replaceWith: '[youtube][![Youtube Url:!:http://]!][/youtube]' },
		{ separator: '---------------' },
		{
			name: 'Size', openWith: '[size=[![Text size]!]]', closeWith: '[/size]',
			dropMenu: [
				{ name: 'Small', openWith: '[size=9]', closeWith: '[/size]' },
				{ name: 'Normal', openWith: '[size=13]', closeWith: '[/size]' },
				{ name: 'Big', openWith: '[size=17]', closeWith: '[/size]' }
			]
		},
		{
			name: 'Colors', className: 'palette', dropMenu: [
				{ name: 'Light Yellow', openWith: '[color=#FCE94F]', closeWith: '[/color]', className: "col1-1" },
				{ name: 'Yellow', openWith: '[color=#EDD400]', closeWith: '[/color]', className: "col1-2" },
				{ name: 'Dark Yellow', openWith: '[color=#C4A000]', closeWith: '[/color]', className: "col1-3" },

				{ name: 'Light Orange', openWith: '[color=#FCAF3E]', closeWith: '[/color]', className: "col2-1" },
				{ name: 'Orange', openWith: '[color=#F57900]', closeWith: '[/color]', className: "col2-2" },
				{ name: 'Dark Orange', openWith: '[color=#CE5C00]', closeWith: '[/color]', className: "col2-3" },

				{ name: 'Light Brown', openWith: '[color=#E9B96E]', closeWith: '[/color]', className: "col3-1" },
				{ name: 'Brown', openWith: '[color=#C17D11]', closeWith: '[/color]', className: "col3-2" },
				{ name: 'Dark Brown', openWith: '[color=#8F5902]', closeWith: '[/color]', className: "col3-3" },

				{ name: 'Light Green', openWith: '[color=#8AE234]', closeWith: '[/color]', className: "col4-1" },
				{ name: 'Green', openWith: '[color=#73D216]', closeWith: '[/color]', className: "col4-2" },
				{ name: 'Dark Green', openWith: '[color=#4E9A06]', closeWith: '[/color]', className: "col4-3" },

				{ name: 'Light Blue', openWith: '[color=#729FCF]', closeWith: '[/color]', className: "col5-1" },
				{ name: 'Blue', openWith: '[color=#3465A4]', closeWith: '[/color]', className: "col5-2" },
				{ name: 'Dark Blue', openWith: '[color=#204A87]', closeWith: '[/color]', className: "col5-3" },

				{ name: 'Light Purple', openWith: '[color=#AD7FA8]', closeWith: '[/color]', className: "col6-1" },
				{ name: 'Purple', openWith: '[color=#75507B]', closeWith: '[/color]', className: "col6-2" },
				{ name: 'Dark Purple', openWith: '[color=#5C3566]', closeWith: '[/color]', className: "col6-3" },

				{ name: 'Light Red', openWith: '[color=#EF2929]', closeWith: '[/color]', className: "col7-1" },
				{ name: 'Red', openWith: '[color=#CC0000]', closeWith: '[/color]', className: "col7-2" },
				{ name: 'Dark Red', openWith: '[color=#A40000]', closeWith: '[/color]', className: "col7-3" },

				{ name: 'White', openWith: '[color=#FFFFFF]', closeWith: '[/color]', className: "col8-1" },
				{ name: 'Gray', openWith: '[color=#BABDB6]', closeWith: '[/color]', className: "col8-3" },
				{ name: 'Black', openWith: '[color=#000000]', closeWith: '[/color]', className: "col9-3" }
			]
		},
		{
			name: 'Emoticon', className: 'emoticon', dropMenu: [
				{ openWith: '[smile=1]', className: "smile-1" },
				{ openWith: '[smile=2]', className: "smile-2" },
				{ openWith: '[smile=3]', className: "smile-3" },
				{ openWith: '[smile=4]', className: "smile-4" },
				{ openWith: '[smile=5]', className: "smile-5" },
				{ openWith: '[smile=6]', className: "smile-6" }
			]
		},
		{ separator: '---------------' },
		{ name: 'Bulleted list', openWith: '[list]\n', closeWith: '\n[/list]' },
		{ name: 'List item', openWith: '[*] ' },
		{ separator: '---------------' },
		{ name: 'Quotes', openWith: '[quote]', closeWith: '[/quote]' },
		{ name: 'Code', openWith: '[code]', closeWith: '[/code]' },
		{ name: 'Spoiler', openWith: '[spoiler]', closeWith: '[/spoiler]' },
		{ name: 'Off top', openWith: '[off]', closeWith: '[/off]' },
		{ name: 'Horizontal line', openWith: '[hr]' },
		{ separator: '---------------' },
		{ name: 'Preview', className: 'preview', call: 'preview' }
	]
}