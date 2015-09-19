<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt" exclude-result-prefixes="msxsl"
                 xmlns:ProductList="ext:ProductList">

  
  <xsl:output method="xml" omit-xml-declaration="yes"/> <!-- para que no ponga lo de la version de xml -->


  
  <xsl:param name="Name" />
  <xsl:param name="Seller" />
  <xsl:param name="Category" />
  <xsl:param name="Price" />
  <xsl:param name="Minutes2End" />
  <xsl:param name="min" />
  
   <xsl:template match="/">
     <table class="table table-bordered table-stripped">
       <thead>
         <tr>
           <th>
             <xsl:value-of select="$Name"/>
           </th>
           <th>
             <xsl:value-of select="$Seller"/>
           </th>
           <th>
             <xsl:value-of select="$Category"/>
           </th>
           <th>
             <xsl:value-of select="$Price"/>
           </th>
           <th>
             <xsl:value-of select="$Minutes2End"/>
           </th>
         </tr>
       </thead>
       <tbody>
        <xsl:apply-templates/>
       </tbody>
     </table>
  </xsl:template>

  <xsl:template match="product">
    <tr>
      <td>
        <xsl:element name ="a">
          <xsl:attribute name="href">
            ./AddFavourite.aspx?id=<xsl:value-of select="@id"/>
          </xsl:attribute>
          <span class="icon-star" ></span>
        </xsl:element>
        &#160;
        <xsl:element name ="a">
          <xsl:attribute name="href">
            ../Comment/AddComment.aspx?id=<xsl:value-of select="@id"/>
          </xsl:attribute>
          <span class="icon-comment" > </span>
        </xsl:element>
        &#160;
        <xsl:element name="a">
          <xsl:attribute name="href">
            ./ProductDetails.aspx?id=<xsl:value-of select="@id"/>
          </xsl:attribute>
          <xsl:value-of select="@name"/>
        </xsl:element>
        <xsl:if test="ProductList:getNumberOfComment(@id) &gt; 0">
          &#160;
          <xsl:element name ="a">
            <xsl:attribute name="href">
              ../Comment/SeeComment.aspx?id=<xsl:value-of select="@id"/>
            </xsl:attribute>
            <span class="icon-list-alt" > </span>
          </xsl:element>
        </xsl:if>
      </td>
      <td>
        <xsl:element name ="a">
          <xsl:attribute name="href">
            ./AddValuation.aspx?id=<xsl:value-of select="@id"/>
          </xsl:attribute>
          <span class="icon-check" > </span>
        </xsl:element>
        &#160;
        <xsl:element name="a">
          <xsl:attribute name="href">
            ../Valuation/SeeValuation.aspx?seller=<xsl:value-of select="@seller"/>
          </xsl:attribute>
          <xsl:value-of select="@seller"/>
        </xsl:element>
      </td>
      <td>
        <xsl:value-of select="@category"/>
      </td>
      <td>
        <xsl:value-of select="@price"/>
      </td>
      <td>
        <xsl:value-of select="@minutesToEnd"/>

        <xsl:value-of select="$min"/>
      </td>
    </tr>
  </xsl:template>
  
  
  
</xsl:stylesheet>
