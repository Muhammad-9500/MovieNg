import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError, map } from 'rxjs/operators';
import { Movie } from '../__models/Movie';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class MovieService {
  selectedMovie: Movie | null;
  baseUrl = environment.apiUrl;
  title: string | null;
  movies: Movie[];

  constructor(private http: HttpClient) { this.selectedMovie = null; this.title = null; this.movies = []; }

  searchMovie(title: string): Observable<Movie[]> {
    const url = `${this.baseUrl}movie?title=${encodeURIComponent(this.title!)}`;
    return this.http.get<Movie[]>(url).pipe(
      catchError((error: HttpErrorResponse) => {
        console.error('Error:', error);
        return throwError('An error occurred while searching for movies');
      })
    );
  }

  toggleExtendedInfo(movie: Movie) {
    if (this.selectedMovie === movie) {
      this.selectedMovie = null; // Deselect the movie if it's already selected
    } else {
      this.selectedMovie = movie; // Select the clicked movie
    }
  }
}
