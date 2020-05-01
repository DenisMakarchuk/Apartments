import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppRoutingModule } from './app-routing.module';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { AuthInterceptor } from './auth/authconfig.interceptor';

import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { ApartmentSearchService } from './core/nswag.generated.service';
import { UserService } from 'src/app/core/nswag.generated.service';

import { AppComponent } from './app.component';
import { MenuComponent } from './menu/menu.component';
import { ApartmentSearchComponent } from './apartment-search/apartment-search.component';
import { ProfileComponent } from './profile/profile.component';
import { LoginComponent } from './auth/login/login.component';
import { OrderDetailComponent } from './userCapabilities/order-detail/order-detail.component';
import { ApartmentDetailComponent } from './userCapabilities/apartment-detail/apartment-detail.component';
import { CommentDetailComponent } from './userCapabilities/comment-detail/comment-detail.component';
import { RegisterComponent } from './auth/register/register.component';



@NgModule({
  declarations: [
    AppComponent,
    MenuComponent,
    ApartmentSearchComponent,
    ProfileComponent,
    LoginComponent,
    OrderDetailComponent,
    ApartmentDetailComponent,
    CommentDetailComponent,
    RegisterComponent
  ],
  imports: [
    BrowserAnimationsModule,
    BsDatepickerModule.forRoot(),
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule
  ],
  providers: [
    ApartmentSearchService,
    UserService,
    {
      provide: HTTP_INTERCEPTORS,
      useClass: AuthInterceptor,
      multi: true
    }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
