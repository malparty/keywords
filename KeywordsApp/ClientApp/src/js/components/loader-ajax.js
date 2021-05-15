import $ from 'jquery';

/// SectionDataLoader
/// Enable to load section content from ajax query
/// <section data-load-ajax="#URL">#LOADING MSG</section>
class SectionDataLoader {
  initLoader = () => {
    $('section[data-load-ajax]').map((index, elt) => {
      var element = $(elt);
      element.load(element.data('load-ajax'), (rsp, status) => {
        if (status == 'error') {
          element.html('<b>Error:</b> Could not load component');
          element.addClass('text-danger');
        }
      });
    });
  };
}
$(function () {
  new SectionDataLoader().initLoader();
});
