function popup(eventId, userId) {
    if (!window.focus) return true;
    var href = 'PopupPhoto.aspx?eid=' + eventId + '&uid=' + userId;
    window.open(href, 'PopupPhoto', 'width=1000,height=600,scrollbars=yes');
    return false;
}