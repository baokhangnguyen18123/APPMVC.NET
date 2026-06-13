using App.Models;

namespace App.Services;

public class PlanetService : List<PlanetModel>
{
  
public PlanetService()
    {
        Add(new PlanetModel { 
            Id = 1, 
            Name = "Mercury", 
            VnName = "Sao Thủy", 
            Content = "Sao Thủy (0,4 au) là hành tinh gần Mặt Trời nhất và là hành tinh nhỏ nhất trong Hệ Mặt Trời (0,055 khối lượng Trái Đất). Sao Thủy không có vệ tinh tự nhiên, và các đặc trưng địa chất của nó ngoài các hố va chạm còn có các sườn và vách núi. Bầu khí quyển rất mỏng của Sao Thủy bao gồm các nguyên tử bị bức xạ Mặt Trời thổi bốc lên khỏi bề mặt của nó. Lõi sắt tương đối lớn và lớp phủ mỏng của nó cho tới nay vẫn chưa được giải thích một cách đầy đủ." 
        });

        Add(new PlanetModel { 
            Id = 2, 
            Name = "Venus", 
            VnName = "Sao Kim", 
            Content = "Sao Kim (0,7 au) có kích cỡ gần bằng Trái Đất (0,815 khối lượng Trái Đất) và giống Trái Đất ở chỗ có một lớp màng silicat dày bao quanh một lõi sắt, có một bầu khí quyển đáng kể và có hoạt động địa chất bên trong. Tuy nhiên, Sao Kim khô hơn Trái Đất rất nhiều và mật độ bầu khí quyển của nó gấp 90 lần Trái Đất. Sao Kim không có vệ tinh tự nhiên. Nó là hành tinh nóng nhất trong hệ Mặt Trời với nhiệt độ bề mặt trên 400 °C, nguyên nhân chủ yếu là do hiệu ứng nhà kính của bầu khí quyển." 
        });

        Add(new PlanetModel { 
            Id = 3, 
            Name = "Earth", 
            VnName = "Trái Đất", 
            Content = "Trái Đất (1 au) là hành tinh lớn nhất và có mật độ lớn nhất trong số các hành tinh đất đá, là nơi duy nhất được biết đến có hoạt động địa chất gần đây, và là nơi duy nhất trong vũ trụ được biết đến là có sự sống. Thủy quyển dạng lỏng của nó là duy nhất trong số các hành tinh đất đá. Bầu khí quyển của Trái Đất khác biệt hoàn toàn so với các hành tinh khác, chứa 21% oxy tự do. Nó có một vệ tinh tự nhiên là Mặt Trăng, vệ tinh lớn nhất trong số các vệ tinh của các hành tinh đất đá trong Hệ Mặt Trời." 
        });

        Add(new PlanetModel { 
            Id = 4, 
            Name = "Mars", 
            VnName = "Sao Hỏa", 
            Content = "Sao Hỏa (1,5 au) nhỏ hơn Trái Đất và Sao Kim (0,107 khối lượng Trái Đất). Nó có một bầu khí quyển mỏng với thành phần chủ yếu là cacbon điôxít. Bề mặt của nó rải rác các ngọn núi lửa khổng lồ như Olympus Mons và các thung lũng đứt gãy như Valles Marineris. Sao Hỏa có màu đỏ do có chứa sắt(III) oxit trong đất của nó. Có hai vệ tinh tự nhiên rất nhỏ (Deimos và Phobos), được cho là các tiểu hành tinh bị bắt giữ." 
        });

        Add(new PlanetModel { 
            Id = 5, 
            Name = "Jupiter", 
            VnName = "Sao Mộc", 
            Content = "Sao Mộc (4,95–5,46 au) là hành tinh lớn nhất trong hệ Mặt Trời, nặng hơn 318 lần Trái Đất. Ngoài phần lõi nhỏ bằng đá, khí quyển của Sao Mộc chủ yếu là hydro và heli. Khí quyển Sao Mộc phức tạp, bao gồm nhiều lớp và vòng khí không phân chia rõ ràng. Do trong lòng Sao Mộc có nhiệt độ cao nên trên bề mặt luôn có bão. Trên bề mặt có cơn bão xoáy khổng lồ có đường kính gấp 3 lần Trái Đất, được gọi là Vết Đỏ Lớn. Sao Mộc có từ quyển rất mạnh, đủ để bẻ hướng bức xạ ion hóa từ Mặt Trời và tạo ra cực quang. Sao Mộc có 95 vệ tinh tự nhiên đã được phát hiện." 
        });

        Add(new PlanetModel { 
            Id = 6, 
            Name = "Saturn", 
            VnName = "Sao Thổ", 
            Content = "Sao Thổ (9,5 au) nổi bật với hệ vành đai kích thước lớn, và có những đặc điểm tương tự với Sao Mộc, chẳng hạn như thành phần khí quyển và từ quyển. Dù thể tích của Sao Thổ bằng 60% thể tích của Sao Mộc, nó chỉ nặng bằng chưa tới 1/3 Sao Mộc (95 lần khối lượng Trái Đất), khiến nó là hành tinh có mật độ nhỏ nhất trong Hệ Mặt Trời. Sao Thổ có 146 vệ tinh tự nhiên đã được phát hiện. Hai trong số đó, Titan và Enceladus, cho thấy các dấu hiệu của hoạt động địa chất, mặc dù chúng được tạo thành chủ yếu bằng băng." 
        });

        Add(new PlanetModel { 
            Id = 7, 
            Name = "Uranus", 
            VnName = "Sao Thiên Vương", 
            Content = "Sao Thiên Vương (19,2 au) là hành tinh nhẹ nhất trong các hành tinh vòng ngoài (14 lần khối lượng Trái Đất). Đặc điểm độc nhất của nó là nó quay quanh Mặt Trời với trục quay nghiêng gần 90 độ so với mặt phẳng quỹ đạo của nó. Lõi của nó lạnh hơn nhiều so với các hành tinh khí khổng lồ khác và bức xạ rất ít nhiệt vào không gian. Sao Thiên Vương có 28 vệ tinh tự nhiên đã được biết đến, lớn nhất là Titania, Oberon, Umbriel, Ariel và Miranda." 
        });

        Add(new PlanetModel { 
            Id = 8, 
            Name = "Neptune", 
            VnName = "Sao Hải Vương", 
            Content = "Sao Hải Vương (30 au) dù hơi nhỏ hơn Sao Thiên Vương nhưng nặng hơn (tương đương 17 lần Trái Đất) và do đó đặc hơn. Nó bức xạ nhiều nhiệt lượng hơn nhưng không bằng Sao Mộc hay Sao Thổ. Sao Hải Vương có 16 vệ tinh tự nhiên đã được biết đến. Lớn nhất là Triton, hoạt động địa chất với các mạch phun nitơ lỏng. Triton là vệ tinh tự nhiên lớn duy nhất có quỹ đạo nghịch hành." 
        });
    }
}

