using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight;
using erlauncher.Services;
using erlauncher.ViewModels;
using erlauncher.Models;
namespace erlauncher
{
    /// <summary>
    /// ViewModelLocator class provides instance resolution for view models and services.
    /// </summary>
    public class ViewModelLocator
    {
        /// <summary>
        /// Initializes a new instance of the ViewModelLocator class.
        /// </summary>
        public ViewModelLocator()
        {
            // Configure IoC container

            // Register services
            SimpleIoc.Default.Register<IGroupService, GroupService>();
            SimpleIoc.Default.Register<IGameInfoService, GameInfoService>();
            SimpleIoc.Default.Register<IScreenshotService, ScreenshotService>();
            SimpleIoc.Default.Register<ITweetService, TweetService>();
            SimpleIoc.Default.Register<ISettingsService, SettingsService>();

            // Register view models
            SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register<AddGroupViewModel>();
            SimpleIoc.Default.Register<CreateGameInfoViewModel>();
            SimpleIoc.Default.Register<TweetViewModel>();
        }

        /// <summary>
        /// Gets the MainViewModel instance.
        /// </summary>
        public MainViewModel MainViewModel => SimpleIoc.Default.GetInstance<MainViewModel>();

        /// <summary>
        /// Gets the AddGroupViewModel instance.
        /// </summary>
        public AddGroupViewModel AddGroupViewModel => SimpleIoc.Default.GetInstance<AddGroupViewModel>();

        /// <summary>
        /// Gets the CreateGameInfoViewModel instance.
        /// </summary>
        public CreateGameInfoViewModel CreateGameInfoViewModel => SimpleIoc.Default.GetInstance<CreateGameInfoViewModel>();

        /// <summary>
        /// Gets the TweetViewModel instance.
        /// </summary>
        public TweetViewModel TweetViewModel => SimpleIoc.Default.GetInstance<TweetViewModel>();

        /// <summary>
        /// Performs cleanup of the view models.
        /// </summary>
        public static void Cleanup()
        {
            SimpleIoc.Default.Reset();
        }
    }
}