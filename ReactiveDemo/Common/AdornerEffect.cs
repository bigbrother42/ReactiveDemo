namespace ReactiveDemo.Common
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Documents;
    using System.Windows.Media;

    public class AdornerEffect : Adorner
    {
        //添付プロパティの実装
        public static readonly DependencyProperty TemplateProperty =
            DependencyProperty.RegisterAttached("Template",
                                                typeof(ControlTemplate), typeof(AdornerEffect),
                                                new FrameworkPropertyMetadata(null, OnTemplateChanged));
        public static ControlTemplate GetTemplate(DependencyObject obj)
        {
            return (ControlTemplate)obj.GetValue(TemplateProperty);
        }
        public static void SetTemplate(DependencyObject obj, ControlTemplate value)
        {
            obj.SetValue(TemplateProperty, value);
        }

        //テンプレート描画用のControlオブジェクトへの参照
        private FrameworkElement _content;

        //コンストラクタ
        public AdornerEffect(UIElement adornedElement) : base(adornedElement)
        {

        }

        //添付プロパティTemplateの設定時に初期化処理を行う
        private static void OnTemplateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {

            var adorned = d as FrameworkElement;
            if (adorned == null) return;
            var me = new AdornerEffect(adorned);

            //装飾層に登録する
            if (adorned.IsInitialized)
            {
                me.AddToAdornerLayer();
            }
            else
            {
                //初期化中の場合は登録処理を遅延させる
                adorned.Loaded += (_, __) => me.AddToAdornerLayer();
            }

            //子Controlオブジェクトを生成して設定されたテンプレートを設定する
            var t = e.NewValue as ControlTemplate;
            var ctrl = new Control { Template = t };
            me._content = ctrl;
            me.AddVisualChild(ctrl);
            me.AddLogicalChild(ctrl);
            me.InvalidateVisual();
        }

        //装飾層に登録する
        private void AddToAdornerLayer()
        {
            AdornerLayer layer = AdornerLayer.GetAdornerLayer(AdornedElement);
            if (layer == null) return;
            //既存の装飾を除去
            Adorner[] registed = layer.GetAdorners(AdornedElement);
            if (registed != null)
            {
                foreach (var ad in registed)
                {
                    if (ad is AdornerEffect) layer.Remove(ad);
                }
            }
            //装飾を登録
            layer.Add(this);
            layer.IsHitTestVisible = false;
        }

        //テンプレート中の要素から装飾対象を取得するヘルパーメソッド
        public static UIElement GetAdornedElementFromTemplateChild(FrameworkElement contained)
        {
            var tp = contained.TemplatedParent as FrameworkElement;
            if (tp == null || tp.GetType() != typeof(Control)) return null;
            var me = tp.Parent as AdornerEffect;
            return me?.AdornedElement;
        }

        //サイズ計測処理の実装 (テンプレートの大きさを装飾対象に一致させる)
        protected override Size MeasureOverride(Size constraint)
        {
            return AdornedElement.RenderSize;
        }

        //配置処理の実装 (テンプレートの位置を装飾対象に一致させる)
        protected override Size ArrangeOverride(Size finalSize)
        {
            _content.Arrange(new Rect(AdornedElement.RenderSize));
            return AdornedElement.RenderSize;
        }

        //描画されるために不可欠なので実装をしておく
        protected override int VisualChildrenCount { get { return 1; } }

        protected override Visual GetVisualChild(int index)
        {
            return _content;
        }
    }
}