﻿using System.Linq;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Messaging;
using Popcorn.Helpers;
using Popcorn.Messaging;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Threading;

namespace Popcorn.ViewModel.Tabs
{
    /// <summary>
    /// The greatest movies tab
    /// </summary>
    public class GreatestTabViewModel : TabsViewModel
    {
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the GreatestTabViewModel class.
        /// </summary>
        public GreatestTabViewModel()
        {
            RegisterMessages();

            RegisterCommands();

            TabName = LocalizationProviderHelper.GetLocalizedValue<string>("GreatestTitleTab");

            if (!Movies.Any())
            {
                DispatcherHelper.CheckBeginInvokeOnUI(async () => await LoadNextPageAsync());
            }
        }

        #endregion

        #region Methods

        #region Method -> RegisterMessages

        /// <summary>
        /// Register messages
        /// </summary>
        private void RegisterMessages()
        {
            Messenger.Default.Register<ChangeLanguageMessage>(
                this,
                language => { TabName = LocalizationProviderHelper.GetLocalizedValue<string>("GreatestTitleTab"); });
        }

        #endregion

        #region Method -> RegisterCommands

        /// <summary>
        /// Register commands
        /// </summary>
        private void RegisterCommands()
        {
            ReloadMovies = new RelayCommand(async () =>
            {
                await LoadNextPageAsync();
                Messenger.Default.Send(new ConnectionErrorMessage(string.Empty, true));
            });
        }

        #endregion

        #region Method -> LoadNextPageAsync

        /// <summary>
        /// Load next page with an optional search parameter
        /// </summary>
        public async Task LoadNextPageAsync()
        {
            Page++;
            IsLoadingMovies = true;
            try
            {
                var movieResults =
                    await MovieService.GetTopRatedMoviesAsync(Page,
                        MaxMoviesPerPage,
                        CancellationLoadNextPageToken.Token);
                var movies = movieResults.ToList();

                foreach (var movie in movies)
                {
                    Movies.Add(movie);
                }

                await UserDataService.ComputeMovieHistoryAsync(movies);
                await MovieService.DownloadCoverImageAsync(movies);
                if (!Movies.Any() && !movies.Any())
                {
                    IsMovieFound = false;
                }
                else
                {
                    IsMovieFound = true;
                }
            }
            catch
            {
                IsMovieFound = Movies.Any();
                Page--;
            }
            finally
            {
                IsLoadingMovies = false;
            }
        }

        #endregion

        #endregion
    }
}