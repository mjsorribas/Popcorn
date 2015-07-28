﻿using System;
using GalaSoft.MvvmLight.Messaging;
using Popcorn.Model.Movie;

namespace Popcorn.Messaging
{
    /// <summary>
    /// Used to broadcast when a movie is buffered
    /// </summary>
    public class MovieBufferedMessage : MessageBase
    {
        #region Properties

        #region Property -> MovieUri
        /// <summary>
        /// The Uri of the buffered movie
        /// </summary>
        public Uri MovieUri { get; private set; }
        #endregion

        #region Property -> Movie
        /// <summary>
        /// The buffered movie
        /// </summary>
        public MovieFull Movie { get; private set; }
        #endregion

        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="movie">The buffered movie</param>
        /// <param name="movieUri">The Uri of the buffered movie</param>
        public MovieBufferedMessage(MovieFull movie, Uri movieUri)
        {
            Movie = movie;
            MovieUri = movieUri;
        }
        #endregion
    }
}
