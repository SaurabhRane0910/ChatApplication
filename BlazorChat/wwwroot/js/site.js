﻿function scrollToBottom(elementId) {
    var el = document.getElementById(elementId);
    if (el) {
        el.scrollTop = el.scrollHeight;
    }
}
