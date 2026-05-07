import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css'],
  standalone: false
})
export class LoginComponent {
  email: string = '';
  password: string = '';
  errorMessage: string = '';

  constructor(private http: HttpClient, private router: Router) {}

  onLogin(): void {
    this.http.post<any>('/api/auth/login', { email: this.email, password: this.password })
      .subscribe({
        next: (response) => {
          localStorage.setItem('accessToken', response.accessToken);
          localStorage.setItem('refreshToken', response.refreshToken);
          localStorage.setItem('expiry', response.expiry);
          this.router.navigate(['/documentation']);
        },
        error: (error) => {
          this.errorMessage = error.error || 'Erro no login';
        }
      });
  }
}