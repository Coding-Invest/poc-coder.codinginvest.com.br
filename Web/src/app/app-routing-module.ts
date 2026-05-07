import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { DocumentationComponent } from './documentation.component';
import { LoginComponent } from './login.component';

const routes: Routes = [
  { path: 'documentation', component: DocumentationComponent },
  { path: 'login', component: LoginComponent },
  { path: '', component: DocumentationComponent }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }