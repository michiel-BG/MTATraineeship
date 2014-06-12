using System; 
using System.Security; 
using System.ComponentModel; 

using System.Windows; 
using System.Windows.Media; 
using System.Windows.Threading; 
using System.Windows.Controls; 
using System.Windows.Controls.Primitives; 

namespace BlogCase
{
    /// <summary> 
    ///Extends Popup control with added behaviors. Used to make sure the template-TipTool is not transparent
   /// </summary> 
    public class PopupBehavior
    {
        public static readonly DependencyProperty AllowsTransparencyProperty =
            DependencyProperty.RegisterAttached(
            "AllowsTransparency",
            typeof(Boolean),
            typeof(PopupBehavior),
            new FrameworkPropertyMetadata(
                true,
                new PropertyChangedCallback(OnAllowsTransparencyChanged),
                new CoerceValueCallback(CoerceAllowsTransparency)));


        public static Boolean GetAllowsTransparency(Control element)
        {
            CheckElementType(element);
            return (Boolean)element.GetValue(AllowsTransparencyProperty);
        }

        public static void SetAllowsTransparency(Control element, Boolean value)
        {
            CheckElementType(element);
            element.SetValue(AllowsTransparencyProperty, value);
        }

        private static Object CoerceAllowsTransparency(DependencyObject element, object baseValue)
        {
            //WPF will force the Popup into WS_CHILD window when running under partial trust, layered windows 
            //is only supported for top level windows, so it doesn't make any sense to set the AllowsTransparency to true 
            //when running under partial trust. 
            return IsRunningUnderFullTrust() ? baseValue : false;
        }

        private static Boolean IsRunningUnderFullTrust()
        {
            Boolean isRunningUnderFullTrust = true;
            try
            {
                NamedPermissionSet permissionSet = new NamedPermissionSet("FullTrust");
                permissionSet.Demand();
            }
            catch (SecurityException)
            {
                isRunningUnderFullTrust = false;
            }

            return isRunningUnderFullTrust;
        }

        private static void OnAllowsTransparencyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            Control element = (Control)sender;
            CheckElementType(element);

            if (element.IsLoaded)
            {
                //Find the Popup logical element root. 
                Popup popup = GetPopupFromVisualChild(element);
                if (popup != null) popup.SetValue(Popup.AllowsTransparencyProperty, e.NewValue);
            }
            else
            {
                var templateDescriptor = DependencyPropertyDescriptor.FromProperty(Control.TemplateProperty, element.GetType());

                EventHandler handler = null;
                handler = (obj, args) =>
               {
                   //Not clear why the BeginInvoke call is needed here, but this could effectively 
                   //workaround cyclic reference exception when evaluating ToolTip/ContextMenu's Style property. 
                   element.Dispatcher.BeginInvoke(DispatcherPriority.Send, new Action(delegate
                   {
                       SetAllowsTransparencyInternal(element, e.NewValue);
                   }));

                   //Clear event handler to avoid resource leak. 
                   templateDescriptor.RemoveValueChanged(element, handler);
               };

                templateDescriptor.AddValueChanged(element, handler);
            }
        }

        private static void CheckElementType(Control element)
        {
            if (!(element is ToolTip || element is ContextMenu))
            {
                throw new NotSupportedException("AllowsTransparency attached property can only be applied to ToolTip or ContextMenu");
            }
        }

        private static void SetAllowsTransparencyInternal(Control element, Object value)
        {
            ToolTip tooTip = element as ToolTip;
            ContextMenu contextMenu = element as ContextMenu;

            // Set the IsOpen property to true to let the ToolTip/ContextMenu create Popup instance early, since 
            // we are only interesting in Popup.AllowsTransparency property rather than 
            // opening the ToolTip/ContextMenu, set its Visibility to Collapsed. 
            element.Visibility = Visibility.Collapsed;
            if (tooTip != null)
            {
                tooTip.IsOpen = true;
            }
            else if (contextMenu != null)
            {
                contextMenu.IsOpen = true;
            }

            //Find the Popup logical element root. 
            Popup popup = GetPopupFromVisualChild(element);
            if (popup != null) popup.SetValue(Popup.AllowsTransparencyProperty, value);

            //Set properties back to what it is initially. 
            if (tooTip != null)
            {
                tooTip.ClearValue(ToolTip.IsOpenProperty);
            }
            else if (contextMenu != null)
            {
                contextMenu.ClearValue(ToolTip.IsOpenProperty);
            }
            element.ClearValue(FrameworkElement.VisibilityProperty);
        }

        private static Popup GetPopupFromVisualChild(Visual child)
        {
            Visual parent = child;
            FrameworkElement visualRoot = null;

            //Traverse the visual tree up to find the PopupRoot instance. 
            while (parent != null)
            {
                visualRoot = parent as FrameworkElement;
                parent = VisualTreeHelper.GetParent(parent) as Visual;
            }

            Popup popup = null;

            // Examine the PopupRoot's logical parent to get the Popup instance. 
            // This might break in the future since it relies on the internal implementation of Popup's element tree. 
            if (visualRoot != null)
            {
                popup = visualRoot.Parent as Popup;
            }

            return popup;
        }
    }
}
