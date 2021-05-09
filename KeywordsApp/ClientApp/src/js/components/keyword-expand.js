class KeywordExpand {
  initLoader = () => {
    $('button[data-toggle=expandview').click(toggleExpandView);
  };
}

toggleExpandView = () => {
  const isExpanded = $('.keyword-body').first().hasClass('card-body');
  if (isExpanded) {
    // Need to close
    $('.keyword-body').removeClass('card-body');
    $('.keyword').removeClass('card');
    $('.keyword .card-text').hide();
    $('button[data-toggle=expandview').html(
      '<i class="fas fa-tachometer-alt"></i> Show results'
    );
    return;
  }
  // Need to expand
  $('.keyword-body').addClass('card-body');
  $('.keyword').addClass('card');
  $('.keyword .card-text').show();
  $('button[data-toggle=expandview').html(
    '<i class="fas fa-eye-slash"></i> Hide results'
  );
};

$(function () {
  new KeywordExpand().initLoader();
});
