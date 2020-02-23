(function() {
    var adjustmentDelegates = [];
    function AddAdjustmentDelegate(adjustmentDelegate) {
        adjustmentDelegates.push(adjustmentDelegate);
    }
    function onControlsInitialized(s, e) {
        adjustPageControls();
    }
    function onBrowserWindowResized(s, e) {
        adjustPageControls();
    }
    function adjustPageControls() {
        for(var i = 0; i < adjustmentDelegates.length; i++) {
            adjustmentDelegates[i]();
        }
    }
    function onLeftMenuItemClick(s, e) {
        if(e.item.name === "ToggleLeftPanel")
            toggleLeftPanel();
        if(e.item.name === "Back")
            window.history.back();
    }

    function onRightMenuItemClick(s, e) {
        if(e.item.name === "ToggleRightPanel") {
            toggleRightPanel();
            e.processOnServer = false;
        }

        if(e.item.name === "AccountItem")
            e.processOnServer = false;
    }

    function HideLeftPanelIfRequired() {
        if (leftPanel.IsExpandable() && leftPanel.IsExpanded())
            leftPanel.Collapse();
    }
    function toggleLeftPanel() {
        if(leftPanel.IsExpandable()) {
            leftPanel.Toggle();
        }
        else {
            leftPanel.SetVisible(!leftPanel.GetVisible());
            adjustPageControls();
        }
    }
    function toggleRightPanel() {
        rightPanel.Toggle();
    }

    function onRightPanelCollapsed(s, e) {
        rightAreaMenu.GetItemByName("ToggleRightPanel").SetChecked(false);
    }

    function onPageToolbarInit(s, e) {
        var adjustmentMethod = function() {
            document.getElementById("pageContent").style.paddingTop = s.GetHeight() + "px"; 
        };
        AddAdjustmentDelegate(adjustmentMethod);
    }

    function onLeftPanelInit(s, e) {
        var adjustmentMethod = function() {
            var pageToolbarPanel = ASPx.GetControlCollection().GetByName("pageToolbarPanel");
            if(pageToolbarPanel)
                pageToolbarPanel.GetMainElement().style.left = s.GetWidth() + "px";

            var toggleButton = leftAreaMenu.GetItemByName("ToggleLeftPanel");
            if(s.IsExpandable())
                toggleButton.SetChecked(leftPanel.IsExpanded());
            else {
                if(leftPanel.GetVisible())
                    document.body.style.marginLeft = "1px";
                else
                    document.body.style.marginLeft = 0;
                toggleButton.SetChecked(leftPanel.GetVisible());
            }
        };
        AddAdjustmentDelegate(adjustmentMethod);
    }

    function onLeftPanelCollapsed(s, e) {
        leftAreaMenu.GetItemByName("ToggleLeftPanel").SetChecked(false);
    }

    window.onControlsInitialized = onControlsInitialized;
    window.onBrowserWindowResized = onBrowserWindowResized;
    window.onLeftMenuItemClick = onLeftMenuItemClick;
    window.onRightMenuItemClick = onRightMenuItemClick;
    window.onRightPanelCollapsed = onRightPanelCollapsed;
    window.onPageToolbarInit = onPageToolbarInit;
    window.onLeftPanelInit = onLeftPanelInit;
    window.onLeftPanelCollapsed = onLeftPanelCollapsed;
    window.adjustPageControls = adjustPageControls;

    window.HideLeftPanelIfRequired = HideLeftPanelIfRequired;
    window.AddAdjustmentDelegate = AddAdjustmentDelegate;
})();