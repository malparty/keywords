class SectionDataLoader {
  /**
   * SectionDataLoader
   * Enable to load content from ajax query
   * @param  {object} container [Jquery object to receive html content]
   */
  constructor(container) {
    container.map((index, elt) => {
      var element = $(elt);
      element.load(element.data('load-ajax'), (rsp, status) => {
        if (status == 'error') {
          element.html('<b>Error:</b> Could not load component');
          element.addClass('text-danger');
        }
      });
    });
  }
}
$(function () {
  const section = $('section[data-load-ajax]');
  new SectionDataLoader(section);
});
