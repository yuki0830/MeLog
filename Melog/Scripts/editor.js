$(function () {
    marked.setOptions({
        langPrefix: ''
    });

    $('#description-box').keyup(function () {
        var src = $(this).val();

        var html = marked(src);

        $('#result').html(html);

        $('pre code').each(function (i, block) {
            hljs.highlightBlock(block);
        });
    });
});