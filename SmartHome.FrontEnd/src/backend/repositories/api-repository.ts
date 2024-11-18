import {
    HttpClient,
    HttpErrorResponse,
    HttpHeaders,
  } from '@angular/common/http';
  import { Observable, catchError, retry, throwError } from 'rxjs';
  
  export default abstract class ApiRepository {
    protected get fullEndpoint(): string {
        return `${this._apiOrigin}/${this._endpoint}`;
    }
  
    protected get headers() {
      return {
        headers: new HttpHeaders({
          accept: 'application/json',
          Authorization: localStorage.getItem('token') ?? '',
        }),
      };
    }
  
    constructor(
      protected readonly _apiOrigin: string,
      protected readonly _endpoint: string,
      protected readonly _http: HttpClient
    ) {}
  
    protected get<T>(extraResource = '', query = ''): Observable<T> {
      query = query ? `?${query}` : '';
      extraResource = extraResource ? `/${extraResource}` : '';
      return this._http
        .get<T>(`${this.fullEndpoint}${extraResource}${query}`, this.headers)
        .pipe(retry(3), catchError(this.handleError));
    }
  
    protected post<T>(body: any, extraResource = ''): Observable<T> {
      extraResource = extraResource ? `/${extraResource}` : '';
  
      return this._http
        .post<T>(`${this.fullEndpoint}${extraResource}`, body, this.headers)
        .pipe(retry(3), catchError(this.handleError));
    }
  
    protected putById<T>(
      id: string,
      body: any = null,
      extraResource = ''
    ): Observable<T> {
      extraResource = extraResource ? `/${extraResource}` : '';
  
      return this._http
        .put<T>(`${this.fullEndpoint}/${id}${extraResource}`, body, this.headers)
        .pipe(retry(3), catchError(this.handleError));
    }

    protected patch<T>(
      extraResource = '',
      body: any = null
    ): Observable<T> {
      const url = `${this.fullEndpoint}${extraResource ? `/${extraResource}` : ''}`;
      console.log('Sending PATCH request:', url, body, this.headers);
      return this._http
        .patch<T>(url, body, this.headers)
        .pipe(retry(3), catchError(this.handleError));
    }

    protected patchById<T>(
        id: string,
        body: any = null,
        extraResource = ''
      ): Observable<T> {
        extraResource = extraResource ? `/${extraResource}` : '';
        return this._http
          .patch<T>(`${this.fullEndpoint}/${id}${extraResource}`, body, this.headers)
          .pipe(catchError(this.handleError));
    }
  
    //Volver
    protected delete<T>(extraResource = '', query = ''): Observable<T> {
      query = query ? `?${query}` : '';
  
      extraResource = extraResource ? `/${extraResource}` : '';
  
      return this._http
        .delete<T>(`${this.fullEndpoint}${extraResource}${query}`, this.headers)
        .pipe(retry(3), catchError(this.handleError));
    }
  
    protected handleError(error: HttpErrorResponse) {
      if (error.error instanceof ErrorEvent) {
        // A client-side or network error occurred. Handle it accordingly.
        console.error('An error occurred:', error.error.message);
      } else {
        // The backend returned an unsuccessful response code.
        // The response body may contain clues as to what went wrong.
        console.error(
          `Backend returned code ${error.status}, ` + `body was: ${error.error}`
        );
      }
      // Return an observable with a user-facing error message.
      return throwError('Something bad happened; please try again later.');
    }
  }
  