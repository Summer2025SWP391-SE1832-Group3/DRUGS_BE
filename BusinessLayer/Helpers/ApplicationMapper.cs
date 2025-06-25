using AutoMapper;
using DataAccessLayer.Dto.Account;
using DataAccessLayer.Dto.BlogPost;
using DataAccessLayer.Dto.Survey;
using DataAccessLayer.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Helpers
{
    public class ApplicationMapper : Profile
    {
        public ApplicationMapper() 
        {
            CreateMap<ApplicationUser, AccountViewDto>();
            CreateMap<Blog, BlogViewDto>()
                .ForMember(dest => dest.PostedBy, opt => opt.MapFrom(src => src.PostedBy.UserName))
                .ForMember(dest => dest.Comments, opt => opt.MapFrom(src => src.Comments.Select(c => new CommentViewDto
                {
                    CommentId = c.CommentId,
                    Content = c.Content,
                    CommentAt = c.CommentAt,
                    UserId = c.UserId,
                    UserName = c.User.UserName,
                    BlogId = c.BlogId,

                }).ToList()))
                .ForMember(dest => dest.BlogImages, opt => opt.MapFrom(src => src.BlogImages.Select(img => img.ImageUrl).ToList()));

            CreateMap<Comment, CommentViewDto>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.UserName));
            CreateMap<CommentUpdateDto, Comment>();
            CreateMap<CommentCreateDto, Comment>();

            CreateMap<Survey, SurveyViewDto>();
            CreateMap<SurveyQuestion, SurveyQuestionViewDto>();
            CreateMap<SurveyAnswer, SurveyAnswerViewDto>();

            //CreateMap<SurveyCreateDto, Survey>().ReverseMap();
            CreateMap<SurveyUpdateWithQuesAndAnsDto, Survey>().ReverseMap();
            CreateMap<SurveyAnswerCreateDto, SurveyAnswer>().ReverseMap();
            CreateMap<SurveyAnswerUpdateDto, SurveyAnswer>().ReverseMap();
            CreateMap<SurveyQuestionCreateDto, SurveyQuestion>().ReverseMap();
            CreateMap<SurveyQuestionUpdateDto, SurveyQuestion>().ReverseMap();

            CreateMap<SurveyResultDto, SurveyResult>();
            CreateMap<SurveyAnswerResultDto, SurveyAnswerResult>();
        }
    }
}
