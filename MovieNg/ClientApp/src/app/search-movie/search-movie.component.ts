import { Component } from '@angular/core';
import { environment } from '../../environments/environment';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Movie } from '../__models/Movie';
import { catchError, map } from 'rxjs/operators';
import { MovieService } from '../_services/MovieService';
import { throwError } from 'rxjs';

@Component({
  selector: 'app-search-movie',
  templateUrl: './search-movie.component.html',
  styleUrls: ['./search-movie.component.css']
})
export class SearchMovieComponent {
  title!: string;
 
  movies!: Movie[];
  selectedMovie!: Movie |null;
  baseUrl = environment.apiUrl;
  errorMessage!: string | null;  
  constructor(private http:HttpClient, private movieService: MovieService) { }

  onSearchMovie() {
    console.log('Searching for movie:', this.title);
    const url = `${this.baseUrl}movie?title=${encodeURIComponent(this.title)}`;

    this.http.get<Movie[]>(url).pipe(
      catchError((error: HttpErrorResponse) => {
        //console.error('Error:', error);
        if (error.status === 404) {
          this.errorMessage = 'Movie not found';
        } else if (error.status === 500) {
          this.errorMessage = 'Server error. Please try again later.';
        } else {
          this.errorMessage = 'An error occurred while searching for movies';
        }
        return throwError('An error occurred while searching for movies');
      })
    ).subscribe(
      (movieList: Movie[]) => {
        this.movies = movieList;
        console.log('Movie List:', this.movies);
        this.errorMessage = null;
      },
      (error) => {
        
      }
    );
  }



  toggleExtendedInfo(movie: Movie) {
    if (this.selectedMovie === movie) {
      this.selectedMovie = null; 
    } else {
      this.selectedMovie = movie;
    }
  }

  isMovieSelected(movie: Movie) {
    return this.selectedMovie === movie;
  }


}
