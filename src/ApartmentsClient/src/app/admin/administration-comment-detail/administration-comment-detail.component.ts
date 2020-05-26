import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Location } from '@angular/common';
import { CommentAdministrationService,
  CommentDTOAdministration
} from 'src/app/services/nswag.generated.service';
import { LoggedService } from 'src/app/services/logged.service';
import * as jwt_decode from 'jwt-decode';

@Component({
  selector: 'app-administration-comment-detail',
  templateUrl: './administration-comment-detail.component.html',
  styleUrls: ['./administration-comment-detail.component.scss']
})
export class AdministrationCommentDetailComponent implements OnInit {

  errorMessage: string;

  comment: CommentDTOAdministration;
  isUpdating = false;

  constructor(
    private commentService: CommentAdministrationService,
    private location: Location,
    private route: ActivatedRoute,
    private authService: LoggedService
  ) { }

  ngOnInit(): void {
    this.getComment();
  }

  get isAdmin(){
    var token = this.authService.getToken();
        
    var decodedtoken = jwt_decode(token);
    var currentRole = decodedtoken['role'];

    if (currentRole.includes('Admin')) {
      return true;
    }
    return false;
  }

  getComment(){
    this.errorMessage = null;

    if (!this.isAdmin) {
      return;
    }

    const id = this.route.snapshot.paramMap.get('id');
    this.commentService.getCommentById(id)
    .subscribe(comment => this.comment = comment,
      error=>{
   
        if (error.status ===  500) {
          this.errorMessage = "Error 500: Internal Server Error";
        }
        if (error.status ===  400) {
          this.errorMessage = "Error 400: " + error.response;
        }
        if (error.status ===  403) {
          this.errorMessage = "Error 403: You are not authorized";
        }
        if (error.status ===  404) {
          this.errorMessage = "Error 404: " + error.response;
        }
        else{
          this.errorMessage = "An error occurred.";
        }
      })
  }

  updating(){
    this.errorMessage = null;

    this.isUpdating = true;
  }

  save(): void {
    this.errorMessage = null;

    this.commentService.updateComment(this.comment)
      .subscribe(comment => {
        this.comment = comment;
        this.isUpdating = false;
        this.goBack();
    },
    error=>{
 
      if (error.status ===  500) {
        this.errorMessage = "Error 500: Internal Server Error";
      }
      if (error.status ===  400) {
        this.errorMessage = "Error 400: " + error.response;
      }
      if (error.status ===  403) {
        this.errorMessage = "Error 403: You are not authorized";
      }
      else{
        this.errorMessage = "An error occurred.";
      }
    });
  }

  delete(){
    this.errorMessage = null;

    const id = this.route.snapshot.paramMap.get('id');
    this.commentService.deleteCommentById(id)
    .subscribe(()=>this.goBack(),
    error=>{
 
      if (error.status ===  500) {
        this.errorMessage = "Error 500: Internal Server Error";
      }
      if (error.status ===  400) {
        this.errorMessage = "Error 400: " + error.response;
      }
      if (error.status ===  403) {
        this.errorMessage = "Error 403: You are not authorized";
      }
      if (error.status ===  404) {
        this.errorMessage = "Error 404: " + error.response;
      }
      else{
        this.errorMessage = "An error occurred.";
      }
    });
  }

  goBack(){
    this.errorMessage = null;

    this.location.back();
  }

}
