/** fileoverview ef.overlay - jQuery plugin for putting a sexy overlay on top of images
*  @author  EF, Filip Wojcieszyn. Inspired by https://github.com/buildinternet/mosaic
*  @version 0.5
*/
(function ($) {

    /**
    * A plugin for overlaying images with animated HTML
    * @requires jQuery
    * @namespace jQuery
    * @constructor
    * @param {jQuery element} element The jQuery object to which the plugin is attached
    * @param {object} [options=null] Options passed as anonymous object
    * @param {string} [options.event="hover"] Type of event the animation should be attached to.
    * @param {string} [options.animation="fade"] Type of animation to be used.
    * @param {int} [options.speed=300] Animation speed.
    * @param {double} [options.opacity=1] How opaque should the overlay be.
    * @param {int[]} [options.slidePosition=[0,0]] X, Y coords from which animation starts
    */
    $.overlay = function (element, options) {

        /** @ignore */
        var plugin = this;
        /** @ignore */
        var $element = $(element); // reference to the jQuery version of DOM element

        /**
        * Default options.
        * @private
        * @type object
        */
        var defaults = {
            event: "hover",
            animation: "fade",
            slidePosition: [0, 0],
            speed: 300,
            opacity: 1,
            color: "#000"
        };

        /**
        * Default slide X and Y anchors.
        * @private
        * @type object
        */
        var slide = { x: "left", y: "bottom" };

        //merge default settings with user-set settings
        plugin.settings = $.extend({}, defaults, options);

        /**
        * The overlay element.
        * @private
        * @type {jQuery object}
        */
        var $overlay = $(element).find(".overlay");

        /**
        * The background element.
        * @private
        * @type {jQuery object}
        */
        var $background = $(element).find(".overlay-item-background");

        /**
        * Staring slide X and Y coords.
        * @private
        * @type object
        */
        var defaultPosition = {};

        /**
        * Final (target) slide X and Y coords.
        * @private
        * @type object
        */
        var finalPosition = {};

        /**
        * Indicates if overlay is visible or not.
        * @private
        * @type bool
        */
        var isVisible = false;

        /**
        * Anonymous function used to init the plugin (fake "constructor")
        * @private
        * @methodOf $.overlay
        * @name init
        */
        plugin.init = function () {

            _setAnimationProperties();

            $overlay.addClass(plugin.settings.animation);
            defaultPosition[slide.x] = $overlay.css(slide.x) == "auto" ? 0 : $overlay.css(slide.x);
            defaultPosition[slide.y] = $overlay.css(slide.y) == "auto" ? 0 : $overlay.css(slide.y);

            if (plugin.settings.event == "click") {
                $element.click(function (event) {
                    event.preventDefault();
                    if (!isVisible) {
                        plugin.animateIn();
                    } else {
                        plugin.animateOut();
                    }
                });
            } else {
                $element.hover(function () {
                    plugin.animateIn();
                }, function () {
                    plugin.animateOut();
                });
            }

        }

        /**
        * Animates the overlay in.
        * @public
        * @methodOf $.overlay
        * @name animateIn
        */
        plugin.animateIn = function () {
            if (plugin.settings.animation == "fade") {
                _fadeIn();
            } else {
                _slideIn();
            }
        }

        /**
        * Animates the overlay out.
        * @public
        * @methodOf $.overlay
        * @name animateOut
        */
        plugin.animateOut = function () {
            if (plugin.settings.animation == "fade") {
                _fadeOut();
            } else {
                _slideOut();
            }
        }

        /**
        * Animates the overlay to the opposite state.
        * @public
        * @methodOf $.overlay
        * @name toggleAnimation
        */
        plugin.toggleAnimation = function () {
            if (plugin.settings.animation == "fade") {
                if (isVisible)
                    _fadeOut();
                else
                    _fadeIn();
            } else {
                if (isVisible)
                    _slideOut();
                else
                    _slideIn();
            }
        }

        /**
        * Fades the overlay in
        * @private
        * @methodOf $.overlay
        * @name _fadeIn
        */
        var _fadeIn = function () {
            $overlay.stop().fadeTo(plugin.settings.speed, plugin.settings.opacity);
            isVisible = true;
        }

        /**
        * Fades the overlay out
        * @private
        * @methodOf $.overlay
        * @name _fadeOut
        */
        var _fadeOut = function () {
            $overlay.stop().fadeTo(plugin.settings.speed, 0);
            isVisible = false;
        }

        /**
        * Slides the overlay in
        * @private
        * @methodOf $.overlay
        * @name _slideIn
        */
        var _slideIn = function () {
            $overlay.stop().show().css("opacity", plugin.settings.opacity).animate(finalPosition, plugin.settings.speed)
            isVisible = true;
        }

        /**
        * Slides the overlay out
        * @private
        * @methodOf $.overlay
        * @name _slideOut
        */
        var _slideOut = function () {
            $overlay.stop().animate(defaultPosition, plugin.settings.speed, function () { $(this); });
            isVisible = false;
        }

        /**
        * Sets the animation anchors (left/right, top/bottom) based on naimation type.
        * @private
        * @methodOf $.overlay
        * @name _setAnimationProperties
        */
        var _setAnimationProperties = function () {
            switch (plugin.settings.animation) {
                case "slide":
                    slide.x = "left";
                    slide.y = "bottom";
                    break;
                case "topslide":
                    slide.x = "left";
                    slide.y = "top";
                    break;
                case "leftslide":
                    slide.x = "left";
                    slide.y = "top";
                    break;
                case "rightslide":
                    slide.x = "right";
                    slide.y = "top";
                    break;
            }

            finalPosition[slide.x] = plugin.settings.slidePosition[0];
            finalPosition[slide.y] = plugin.settings.slidePosition[1];
        }

        plugin.init();
    }

    /**
    * A plugin for overlaying images with animated HTML, extending jQuery.fn namespace
    * @requires jQuery
    * @namespace jQuery.fn
    * @constructor
    * @param {object} [options=null] Options passed as anonymous object
    * @param {string} [options.event="hover"] Type of event the animation should be attached to.
    * @param {string} [options.animation="fade"] Type of animation to be used.
    * @param {int} [options.speed=300] Animation speed.
    * @param {double} [options.opacity=1] How opaque should the overlay be.
    * @param {int[]} [options.slidePosition=[0,0]] X, Y coords from which animation starts
    */
    $.fn.overlay = function (options) {

        // iterate through the DOM elements we are attaching the plugin to
        return this.each(function () {

            // if plugin has not already been attached to the element
            if (undefined == $(this).data('overlay')) {
                var plugin = new $.overlay(this, options);
                $(this).data('overlay', plugin);
            }
        });
    }
})(jQuery);